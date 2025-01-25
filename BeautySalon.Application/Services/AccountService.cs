namespace BeautySalon.Application.Services
{
    using BeautySalon.Application.Services.Interfaces;
    using BeautySalon.Domain.Enums;
    using BeautySalon.Domain.Models;
    using BeautySalon.Domain.Models.Requests;
    using BeautySalon.Infrastructure.Repositories;
    using BeautySalon.Infrastructure.Repositories.User;
    using Microsoft.Extensions.Logging;
    using System.Security.Claims;

    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRepository<CustomerEntity> _customerRepository;
        private readonly IRepository<EmployeeEntity> _employeeRepository;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AccountService> _logger;

        public AccountService(IUserRepository userRepository, IRepository<CustomerEntity> customerRepository,
            IRepository<EmployeeEntity> employeeRepository, IJwtService jwtService, ILogger<AccountService> logger)
        {
            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<(bool Success, string? Token, string? Message)> LoginAsync(string email, string password)
        {
            _logger.LogInformation("Attempting to log in user with email: {Email}", email);

            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                _logger.LogWarning("Login failed for email: {Email}", email);
                return (false, null, "Invalid email or password");
            }

            var token = _jwtService.GenerateToken(user.Id, user.Role.ToString());
            _logger.LogInformation("Login successful for user: {UserId}", user.Id);

            return (true, token, null);
        }

        public async Task<(bool Success, string? Message)> RegisterCustomerAsync(CustomerRegisterRequest request)
        {
            _logger.LogInformation("Attempting to register a new customer with email: {Email}", request.Email);

            if (await _userRepository.GetByEmailAsync(request.Email) != null)
            {
                _logger.LogWarning("Registration failed. Email already exists: {Email}", request.Email);
                return (false, "A user with this email already exists.");
            }

            var user = new UserEntity
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = UserRole.Customer
            };
            await _userRepository.AddAsync(user);
            _logger.LogInformation("User created with ID: {UserId}", user.Id);

            var customer = new CustomerEntity
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                User = user
            };
            await _customerRepository.AddAsync(customer);
            _logger.LogInformation("Customer profile created with ID: {CustomerId}", customer.Id);

            return (true, null);
        }

        public async Task<(bool Success, string? Message)> RegisterEmployeeAsync(CreateEmployeeRequest request, Guid adminId)
        {
            _logger.LogInformation("Admin {AdminId} is attempting to create a new employee with email: {Email}", adminId, request.Email);

            var admin = await _userRepository.GetByIdAsync(adminId);
            if (admin == null || admin.Role != UserRole.Administrator)
            {
                _logger.LogWarning("Unauthorized attempt to create an employee by user {AdminId}", adminId);
                return (false, "Only administrators can create employees");
            }

            var user = new UserEntity
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = UserRole.Employee,
            };

            await _userRepository.AddAsync(user);
            _logger.LogInformation("UserEntity created with ID: {UserId}", user.Id);

            var employee = new EmployeeEntity
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Specialization = request.Specialization,
                User = user
            };

            await _employeeRepository.AddAsync(employee);
            _logger.LogInformation("Employee profile created with ID: {EmployeeId}", employee.Id);

            return (true, null);
        }

        public CurrentUser? GetCurrentUser(ClaimsPrincipal user)
        {
            var userId = user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var userRole = user.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            if (userId == null || userRole == null)
            {
                _logger.LogWarning("Failed to retrieve current user information");
                return null;
            }

            _logger.LogInformation("Current user retrieved successfully: {UserId}", userId);
            return new CurrentUser
            {
                UserId = Guid.Parse(userId),
                Role = userRole
            };
        }
    }
}
