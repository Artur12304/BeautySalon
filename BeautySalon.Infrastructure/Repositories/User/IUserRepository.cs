namespace BeautySalon.Infrastructure.Repositories.User
{
    using BeautySalon.Domain.Models;

    public interface IUserRepository : IRepository<UserEntity>
    {
        Task<UserEntity?> GetByEmailAsync(string email);
        Task AddCustomerAsync(CustomerEntity customer);
        Task AddEmployeeAsync(EmployeeEntity employee);
    }
}
