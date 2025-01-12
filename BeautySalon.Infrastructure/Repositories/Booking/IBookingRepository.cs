namespace BeautySalon.Infrastructure.Repositories.Booking
{
    using BeautySalon.Domain.Enums;
    using BeautySalon.Domain.Models;

    public interface IBookingRepository : IRepository<BookingEntity>
    {
        Task<IEnumerable<BookingEntity>> GetBookingsByFiltersAsync(DateTime? date, BookingStatus? status);
        Task<IEnumerable<BookingEntity>> GetActiveBookingsByFiltersAsync(DateTime? date);
        Task<bool> IsTimeSlotAvailableAsync(Guid serviceId, DateTime bookingDate, int duration);
        IQueryable<BookingEntity> GetAll();
    }
}
