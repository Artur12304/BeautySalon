namespace BeautySalon.Domain.Models.Dtos
{
    using System;

    public class BookingDetailsDto
    {
        public string CustomerEmail { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public DateTime BookingDate { get; set; }
    }
}
