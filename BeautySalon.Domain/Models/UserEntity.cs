namespace BeautySalon.Domain.Models
{
    using BeautySalon.Domain.Enums;

    public class UserEntity : BaseEntity
    {
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Customer;

        public CustomerEntity Customer { get; set; } = default!;
        public EmployeeEntity Employee { get; set; } = default!;
    }
}