namespace BeautySalon.Domain.Models
{
    using BeautySalon.Domain.Enums;

    public class BookingEntity : BaseEntity
    {
        public DateTime BookingDate { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ServiceId { get; set; }
        public Guid EmployeeId { get; set; }
        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        public CustomerEntity Customer { get; set; } = default!;
        public ServiceEntity Service { get; set; } = default!;
        public EmployeeEntity Employee { get; set; } = default!;
    }
}