namespace BeautySalon.Domain.Models.Requests
{
    using BeautySalon.Domain.Enums;

    public class UpdateBookingStatusRequest
    {
        public Guid Id { get; set; }
        public BookingStatus Status { get; set; }
    }
}
