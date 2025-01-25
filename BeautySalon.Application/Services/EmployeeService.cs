namespace BeautySalon.Application.Services
{
    using BeautySalon.Application.Services.Interfaces;
    using BeautySalon.Domain.Enums;
    using BeautySalon.Domain.Models;
    using BeautySalon.Domain.Models.Dtos;
    using BeautySalon.Infrastructure.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<EmployeeEntity> _employeeRepository;
        private readonly IRepository<ServiceEntity> _serviceRepository;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(IRepository<EmployeeEntity> employeeRepository,
            IRepository<ServiceEntity> serviceRepository, ILogger<EmployeeService> logger)
        {
            _employeeRepository = employeeRepository;
            _serviceRepository = serviceRepository;
            _logger = logger;
        }

        public async Task<int> GetEmployeeCountAsync()
        {
            _logger.LogInformation("Fetching employee count");

            var count = await _employeeRepository.CountAsync();

            _logger.LogInformation("Employee count fetched: {Count}", count);

            return count;
        }

        public async Task<EmployeeEntity> GetEmployeeByUserIdAsync(Guid userId)
        {
            _logger.LogInformation("Fetching employee with UserId: {UserId}", userId);

            var employee = await _employeeRepository.GetAllQueryable()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.UserId == userId);

            if (employee == null)
            {
                _logger.LogWarning("No employee found for UserId: {UserId}", userId);
            }

            return employee;
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync()
        {
            _logger.LogInformation("Fetching all employees");

            var employees = await _employeeRepository.GetAllQueryable()
                .Include(x => x.User)
                .Select(x => new EmployeeDto
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    PhoneNumber = x.PhoneNumber,
                    Specialization = x.Specialization,
                    Email = x.User.Email,
                })
                .ToListAsync();

            return employees;
        }

        public async Task<EmployeeEntity> GetAvailableEmployeeAsync(Guid serviceId, DateTime bookingDate)
        {
            _logger.LogInformation("Searching for available employee for serviceId: {ServiceId} on {BookingDate}", serviceId, bookingDate);

            var service = await _serviceRepository.GetByIdAsync(serviceId);
            if (service == null)
            {
                _logger.LogWarning("ServiceEntity with Id: {ServiceId} not found", serviceId);
                return null;
            }

            DateTime endBookingDate = bookingDate.AddMinutes(service.Duration);

            bookingDate = DateTime.SpecifyKind(bookingDate, DateTimeKind.Utc);
            endBookingDate = DateTime.SpecifyKind(endBookingDate, DateTimeKind.Utc);

            var employees = await _employeeRepository.GetAllQueryable()
                .Include(e => e.Bookings
                    .Where(b => b.BookingDate >= bookingDate.Date && b.BookingDate < bookingDate.Date.AddDays(1) && b.Status != BookingStatus.Cancelled))
                    .ThenInclude(b => b.Service)
                .ToListAsync();

            foreach (var employee in employees)
            {
                var isAvailable = !employee.Bookings.Any(b =>
                {
                    var correctedBookingDate = b.BookingDate.AddHours(1);

                    return (correctedBookingDate < endBookingDate && correctedBookingDate >= bookingDate) ||
                           (correctedBookingDate <= bookingDate && correctedBookingDate.AddMinutes(b.Service.Duration) > bookingDate);
                });

                if (isAvailable)
                {
                    return employee;
                }
            }

            _logger.LogWarning("No available employees found for serviceId: {ServiceId} on {BookingDate}", serviceId, bookingDate);
            return null;
        }

        public async Task<EmployeeEntity> CreateEmployeeAsync(EmployeeEntity employee)
        {
            await _employeeRepository.AddAsync(employee);
            _logger.LogInformation("Employee created successfully with Id: {Id}", employee.Id);
            return employee;
        }

        public async Task<bool> DeleteEmployeeAsync(Guid id)
        {
            _logger.LogInformation("Attempting to delete employee with Id: {Id}", id);

            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
            {
                _logger.LogWarning("Failed to delete employee: Employee not found for Id: {Id}", id);
                return false;
            }

            await _employeeRepository.DeleteAsync(id);
            _logger.LogInformation("Employee deleted successfully with Id: {Id}", id);
            return true;
        }
    }
}