namespace BeautySalon.Domain.Models
{
    public class ServiceEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal DefaultPrice { get; set; }
        public int Duration { get; set; }

        public ICollection<PriceListEntity> PriceLists { get; set; } = new List<PriceListEntity>();
        public ICollection<BookingEntity> Bookings { get; set; } = new List<BookingEntity>();
    }
}