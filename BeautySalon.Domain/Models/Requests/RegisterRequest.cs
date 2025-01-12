namespace BeautySalon.Domain.Models.Requests
{
    using BeautySalon.Domain.Enums;

    public class RegisterRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }
}