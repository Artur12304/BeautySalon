namespace BeautySalon.Domain.Models
{
    public class CurrentUser
    {
        public Guid UserId { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
