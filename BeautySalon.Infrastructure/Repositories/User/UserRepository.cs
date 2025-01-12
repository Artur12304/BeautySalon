namespace BeautySalon.Infrastructure.Repositories.User
{
    using BeautySalon.Domain.Models;
    using BeautySalon.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class UserRepository : Repository<UserEntity>, IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<UserEntity?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddCustomerAsync(CustomerEntity customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task AddEmployeeAsync(EmployeeEntity employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }
    }
}
