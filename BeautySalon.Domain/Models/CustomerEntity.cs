namespace BeautySalon.Domain.Models
{
    public class CustomerEntity : BaseEntity
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public UserEntity User { get; set; } = default!;
        public ICollection<BookingEntity> Bookings { get; set; } = new List<BookingEntity>();
    }
}