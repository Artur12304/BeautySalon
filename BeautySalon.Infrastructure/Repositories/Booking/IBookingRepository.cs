namespace BeautySalon.Infrastructure.Repositories.Booking
{
    using BeautySalon.Domain.Enums;
    using BeautySalon.Domain.Models;
    using BeautySalon.Domain.Models.Dtos;

    public interface IBookingRepository : IRepository<BookingEntity>
    {
        Task<BookingDetailsDto> GetBookingDetailsWithCustomerAndServiceAsync(Guid bookingId);
        Task<IEnumerable<BookingEntity>> GetBookingsByFiltersAsync(DateTime? date, BookingStatus? status);
        Task<IEnumerable<BookingEntity>> GetActiveBookingsByFiltersAsync(DateTime? date);
        Task<bool> IsTimeSlotAvailableAsync(Guid serviceId, DateTime bookingDate, int duration);
        IQueryable<BookingEntity> GetAll();
    }
}
