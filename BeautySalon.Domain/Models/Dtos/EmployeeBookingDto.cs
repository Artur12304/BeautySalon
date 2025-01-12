namespace BeautySalon.Domain.Models.Dtos
{
    using BeautySalon.Domain.Enums;

    public class EmployeeBookingDto
    {
        public Guid Id { get; set; }
        public DateTime BookingDate { get; set; }
        public BookingStatus Status { get; set; }
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public string ServiceDescription { get; set; } = string.Empty;
        public decimal ServicePrice { get; set; }
        public int ServiceDuration { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerFirstName { get; set; } = string.Empty;
        public string CustomerLastName { get; set; } = string.Empty;
        public string CustomerPhoneNumber { get; set; } = string.Empty;
    }
}