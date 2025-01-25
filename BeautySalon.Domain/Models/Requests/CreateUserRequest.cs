namespace BeautySalon.Domain.Models.Requests
{
    using BeautySalon.Domain.Enums;
    using BeautySalon.Domain.Models;

    public class CreateUserRequest
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; }

        public CustomerEntity Customer { get; set; } = default!;
        public EmployeeEntity Employee { get; set; } = default!;
    }
}