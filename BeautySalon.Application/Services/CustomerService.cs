namespace BeautySalon.Application.Services
{
    using BeautySalon.Application.Services.Interfaces;
    using BeautySalon.Domain.Models;
    using BeautySalon.Infrastructure.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class CustomerService : ICustomerService
    {
        private readonly IRepository<CustomerEntity> _customerRepository;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(IRepository<CustomerEntity> customerRepository, ILogger<CustomerService> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }

        public async Task<CustomerEntity> GetCustomerByIdAsync(Guid customerId)
        {
            _logger.LogInformation($"Fetching customer with id: {customerId}");
            return await _customerRepository.GetByIdAsync(customerId);
        }

        public async Task<CustomerEntity> GetCustomerByUserIdAsync(Guid userId)
        {
            _logger.LogInformation($"Fetching customer with UserId: {userId}");

            var customer = await _customerRepository.GetAllQueryable()
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (customer == null)
            {
                _logger.LogWarning($"Customer with UserId: {userId} not found.");
            }

            return customer;
        }

        public async Task<IEnumerable<CustomerEntity>> GetCustomersAsync()
        {
            _logger.LogInformation("Fetching all customers");
            return await _customerRepository.GetAllAsync();
        }

        public async Task<CustomerEntity> CreateCustomerAsync(CustomerEntity customer)
        {
            await _customerRepository.AddAsync(customer);
            _logger.LogInformation("Customer created successfully with Id: {Id}", customer.Id);
            return customer;
        }
    }
}