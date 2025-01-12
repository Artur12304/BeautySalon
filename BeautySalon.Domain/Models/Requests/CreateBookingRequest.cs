namespace BeautySalon.Domain.Models.Requests
{
    public class CreateBookingRequest
    {
        public string BookingDate { get; set; } = string.Empty;
        public string ServiceId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
    }
}