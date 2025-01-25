namespace BeautySalon.Infrastructure.Repositories.Booking
{
    using BeautySalon.Domain.Enums;
    using BeautySalon.Domain.Models;
    using BeautySalon.Domain.Models.Dtos;
    using BeautySalon.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class BookingRepository : Repository<BookingEntity>, IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<BookingDetailsDto> GetBookingDetailsWithCustomerAndServiceAsync(Guid bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.Customer)
                .ThenInclude(c => c.User)
                .Include(b => b.Service)
                .Where(b => b.Id == bookingId)
                .FirstOrDefaultAsync();

            if (booking == null)
            {
                return null;
            }

            var bookingDetails = new BookingDetailsDto
            {
                CustomerEmail = booking.Customer.User.Email,
                ServiceName = booking.Service.Name,
                BookingDate = booking.BookingDate,
            };

            return bookingDetails;
        }

        public async Task<IEnumerable<BookingEntity>> GetBookingsByFiltersAsync(DateTime? date, BookingStatus? status)
        {
            var query = _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Service)
                .Include(b => b.Employee)
                .AsQueryable();

            if (date.HasValue)
            {
                var utcDate = date.Value.ToUniversalTime();
                query = query.Where(b => b.BookingDate.Date == utcDate.Date);
            }

            if (status.HasValue)
            {
                query = query.Where(b => b.Status == status.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<BookingEntity>> GetActiveBookingsByFiltersAsync(DateTime? date)
        {
            var query = _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Service)
                .Include(b => b.Employee)
                .AsQueryable();

            if (date.HasValue)
            {
                var startOfDayUtc = date.Value.Date.ToUniversalTime();
                var endOfDayUtc = startOfDayUtc.AddDays(1).AddTicks(-1);
                query = query.Where(b => b.BookingDate >= startOfDayUtc && b.BookingDate <= endOfDayUtc);
            }

            query = query.Where(b => b.Status != BookingStatus.Cancelled);

            return await query.ToListAsync();
        }

        public IQueryable<BookingEntity> GetAll()
        {
            return _context.Bookings.AsQueryable();
        }

        public async Task<bool> IsTimeSlotAvailableAsync(Guid serviceId, DateTime bookingDate, int duration)
        {
            var endDate = bookingDate.AddMinutes(duration);

            var employees = await _context.Employees
                .Include(e => e.Bookings)
                .ToListAsync();

            foreach (var employee in employees)
            {
                var employeeBookings = employee.Bookings
                    .Where(b => b.ServiceId == serviceId &&
                                ((b.BookingDate < endDate && b.BookingDate >= bookingDate) ||
                                 (b.BookingDate <= bookingDate && b.BookingDate.AddMinutes(b.Service.Duration) > bookingDate)));

                if (employeeBookings.Any())
                {
                    return false;
                }
            }

            return true;
        }
    }
}
