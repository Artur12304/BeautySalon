namespace BeautySalon.Application.Services.Interfaces
{
    using BeautySalon.Domain.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICustomerService
    {
        Task<CustomerEntity> GetCustomerByIdAsync(Guid customerId);
        Task<CustomerEntity> GetCustomerByUserIdAsync(Guid userId);
        Task<IEnumerable<CustomerEntity>> GetCustomersAsync();
        Task<CustomerEntity> CreateCustomerAsync(CustomerEntity customer);
    }
}