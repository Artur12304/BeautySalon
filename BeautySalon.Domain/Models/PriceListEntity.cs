namespace BeautySalon.Domain.Models
{
    public class PriceListEntity : BaseEntity
    {
        public Guid ServiceId { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DayOfWeek? DayOfWeek { get; set; }

        public ServiceEntity Service { get; set; } = default!;
    }
}