namespace BeautySalon.Domain.Models.Requests
{
    public class CreateServiceRequest : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal DefaultPrice { get; set; }
        public int Duration { get; set; }
    }
}