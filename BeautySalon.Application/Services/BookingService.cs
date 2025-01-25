namespace BeautySalon.Application.Services
{
    using BeautySalon.Application.Services.Interfaces;
    using BeautySalon.Domain.Enums;
    using BeautySalon.Domain.Models;
    using BeautySalon.Domain.Models.Dtos;
    using BeautySalon.Domain.Models.Requests;
    using BeautySalon.Infrastructure.Repositories;
    using BeautySalon.Infrastructure.Repositories.Booking;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System;

    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRepository<ServiceEntity> _serviceRepository;
        private readonly IRepository<PriceListEntity> _priceListRepository;
        private readonly IRepository<EmployeeEntity> _employeeRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger<BookingService> _logger;

        public BookingService(IBookingRepository bookingRepository,
                              IRepository<ServiceEntity> serviceRepository,
                              IRepository<PriceListEntity> priceListRepository,
                              IRepository<EmployeeEntity> employeeRepository,
                              IEmailService emailService,
                              ILogger<BookingService> logger)
        {
            _bookingRepository = bookingRepository;
            _serviceRepository = serviceRepository;
            _priceListRepository = priceListRepository;
            _employeeRepository = employeeRepository;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<int> GetBookingCountAsync()
        {
            _logger.LogInformation("Fetching employee count");

            var count = await _bookingRepository.CountAsync();

            _logger.LogInformation("Employee count fetched: {Count}", count);

            return count;
        }

        public async Task<IEnumerable<AdminBookingDto>> GetAdminBookingsAsync(DateTime? startDate, DateTime? endDate, BookingStatus? status)
        {
            var bookingsQuery = _bookingRepository.GetAll();

            if (startDate.HasValue)
            {
                startDate = startDate.Value.Date;
                bookingsQuery = bookingsQuery.Where(b => b.BookingDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                endDate = endDate.Value.Date.AddDays(1).AddMilliseconds(-1);
                bookingsQuery = bookingsQuery.Where(b => b.BookingDate <= endDate.Value);
            }

            if (status.HasValue)
            {
                bookingsQuery = bookingsQuery.Where(b => b.Status == status.Value);
            }

            var bookings = await bookingsQuery
                .Include(b => b.Service)
                .Include(b => b.Employee)
                .Include(b => b.Customer)
                .OrderBy(b => b.BookingDate)
                .Select(b => new AdminBookingDto
                {
                    Id = b.Id,
                    BookingDate = b.BookingDate,
                    Status = b.Status,
                    ServiceId = b.ServiceId,
                    ServiceName = b.Service.Name,
                    ServiceDescription = b.Service.Description,
                    ServicePrice = b.Service.DefaultPrice,
                    ServiceDuration = b.Service.Duration,
                    CustomerId = b.Customer.Id,
                    CustomerFirstName = b.Customer.FirstName,
                    CustomerLastName = b.Customer.LastName,
                    CustomerPhoneNumber = b.Customer.PhoneNumber,
                    EmployeeId = b.Employee.Id,
                    EmployeeFirstName = b.Employee.FirstName,
                    EmployeeLastName = b.Employee.LastName,
                    EmployeePhoneNumber = b.Employee.PhoneNumber
                })
                .ToListAsync();

            return bookings;
        }

        public async Task<IEnumerable<CustomerBookingDto>> GetCustomerBookingsAsync(
            Guid customerId, DateTime? startDate, DateTime? endDate, BookingStatus? status)
        {
            var bookingsQuery = _bookingRepository.GetAll()
                .Where(b => b.CustomerId == customerId);

            if (startDate.HasValue)
            {
                startDate = startDate.Value.Date;
                bookingsQuery = bookingsQuery.Where(b => b.BookingDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                endDate = endDate.Value.Date.AddDays(1).AddMilliseconds(-1);
                bookingsQuery = bookingsQuery.Where(b => b.BookingDate <= endDate.Value);
            }

            if (status.HasValue)
            {
                bookingsQuery = bookingsQuery.Where(b => b.Status == status.Value);
            }

            var bookings = await bookingsQuery
                .Include(b => b.Service)
                .Include(b => b.Employee)
                .OrderBy(b => b.BookingDate)
                .Select(b => new CustomerBookingDto
                {
                    Id = b.Id,
                    BookingDate = b.BookingDate,
                    Status = b.Status,
                    ServiceId = b.ServiceId,
                    ServiceName = b.Service.Name,
                    ServiceDescription = b.Service.Description,
                    ServicePrice = b.Service.DefaultPrice,
                    ServiceDuration = b.Service.Duration,
                    EmployeeId = b.Employee.Id,
                    EmployeeFirstName = b.Employee.FirstName,
                    EmployeeLastName = b.Employee.LastName,
                    EmployeePhoneNumber = b.Employee.PhoneNumber
                })
                .ToListAsync();

            return bookings;
        }

        public async Task<IEnumerable<EmployeeBookingDto>> GetEmployeeBookingsAsync(Guid employeeId, DateTime? startDate, DateTime? endDate, BookingStatus? status)
        {
            var query = _bookingRepository.GetAllQueryable()
                .Where(b => b.EmployeeId == employeeId);

            if (startDate.HasValue)
            {
                startDate = startDate.Value.Date;
                query = query.Where(b => b.BookingDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                endDate = endDate.Value.Date.AddDays(1).AddMilliseconds(-1);
                query = query.Where(b => b.BookingDate <= endDate.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(b => b.Status == status.Value);
            }

            var bookings = await query
                .Include(b => b.Service)
                .Include(b => b.Customer)
                .OrderBy(b => b.BookingDate)
                .Select(b => new EmployeeBookingDto
                {
                    Id = b.Id,
                    BookingDate = b.BookingDate,
                    Status = b.Status,
                    ServiceId = b.ServiceId,
                    ServiceName = b.Service.Name,
                    ServiceDescription = b.Service.Description,
                    ServicePrice = b.Service.DefaultPrice,
                    ServiceDuration = b.Service.Duration,
                    CustomerId = b.CustomerId,
                    CustomerFirstName = b.Customer.FirstName,
                    CustomerLastName = b.Customer.LastName,
                    CustomerPhoneNumber = b.Customer.PhoneNumber
                })
                .ToListAsync();

            return bookings;
        }

        public async Task<BookingEntity?> GetBookingByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching booking with Id: {Id}", id);
            return await _bookingRepository.GetByIdAsync(id);
        }

        public async Task<(bool Conflict, string? Message, Guid? BookingId)> CreateBookingAsync(Guid serviceId, Guid customerId, DateTime bookingDate, Guid employeeId)
        {
            _logger.LogInformation("Creating booking for ServiceId: {ServiceId} on {BookingDate}", serviceId, bookingDate);

            var service = await _serviceRepository.GetByIdAsync(serviceId);
            if (service == null)
            {
                _logger.LogWarning("Failed to create booking: ServiceEntity not found for Id: {ServiceId}", serviceId);
                return (true, "ServiceEntity not found", null);
            }

            bookingDate = bookingDate.ToUniversalTime();

            var isAvailable = await _bookingRepository.IsTimeSlotAvailableAsync(serviceId, bookingDate, service.Duration);
            if (!isAvailable)
            {
                _logger.LogWarning("Failed to create booking: Time slot not available for ServiceId: {ServiceId} on {BookingDate}", serviceId, bookingDate);
                return (true, "The selected time slot is not available.", null);
            }

            var booking = new BookingEntity
            {
                BookingDate = bookingDate,
                ServiceId = serviceId,
                CustomerId = customerId,
                EmployeeId = employeeId,
            };

            booking.Service = service;
            booking.Service.Price = await GetDynamicPrice(booking.ServiceId, booking.BookingDate);

            await _bookingRepository.AddAsync(booking);
            _logger.LogInformation("Booking created successfully for ServiceId: {ServiceId} on {BookingDate}", booking.ServiceId, booking.BookingDate);
            return (false, null, booking.Id);
        }

        public async Task<bool> UpdateBookingStatusAsync(UpdateBookingStatusRequest updateBookingstatus)
        {
            _logger.LogInformation("Updating booking status for Id: {Id} to {Status}", updateBookingstatus.Id, updateBookingstatus.Status);

            var booking = await _bookingRepository.GetByIdAsync(updateBookingstatus.Id);
            if (booking == null)
            {
                _logger.LogWarning("Failed to update booking status: Booking not found for Id: {Id}", updateBookingstatus.Id);
                return false;
            }

            booking.Status = updateBookingstatus.Status;
            await _bookingRepository.UpdateAsync(booking);
            _logger.LogInformation("Booking status updated successfully for Id: {Id}", updateBookingstatus.Id);

            if (updateBookingstatus.Status == BookingStatus.Confirmed)
            {
                var bookingDetails = await _bookingRepository.GetBookingDetailsWithCustomerAndServiceAsync(updateBookingstatus.Id);

                if (bookingDetails != null)
                {
                    var formattedBookingDate = bookingDetails.BookingDate.ToString("dd-MM-yyyy HH:mm");

                    var emailBody = $"Your booking for the service '{bookingDetails.ServiceName}' has been confirmed.\n" +
                                    $"Booking Date: {formattedBookingDate}";

                    await _emailService.SendConfirmationEmailAsync(bookingDetails.CustomerEmail, emailBody);
                }
            }

            return true;
        }

        public async Task<bool> DeleteBookingAsync(Guid id)
        {
            _logger.LogInformation("Deleting booking with Id: {Id}", id);

            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
            {
                _logger.LogWarning("Failed to delete booking: Booking not found for Id: {Id}", id);
                return false;
            }

            await _bookingRepository.DeleteAsync(id);
            _logger.LogInformation("Booking deleted successfully for Id: {Id}", id);
            return true;
        }

        private async Task<decimal> GetDynamicPrice(Guid serviceId, DateTime bookingDate)
        {
            _logger.LogInformation("Calculating dynamic price for ServiceId: {ServiceId} on {BookingDate}", serviceId, bookingDate);

            var priceListEntry = await _priceListRepository.GetAllQueryable()
                .Where(pl => pl.ServiceId == serviceId &&
                             pl.StartDate <= bookingDate &&
                             pl.EndDate >= bookingDate)
                .OrderByDescending(pl => pl.StartDate)
                .FirstOrDefaultAsync();

            if (priceListEntry != null)
            {
                _logger.LogInformation("Dynamic price found: {Price}", priceListEntry.Price);
                return priceListEntry.Price;
            }

            var service = await _serviceRepository.GetByIdAsync(serviceId);
            var defaultPrice = service?.DefaultPrice ?? 0;
            _logger.LogInformation("No dynamic price found. Using default price: {Price}", defaultPrice);
            return defaultPrice;
        }

        public async Task<IEnumerable<string>> GetAvailableTimeSlotsForServiceAsync(DateTime date, Guid serviceId)
        {
            var service = await _serviceRepository.GetByIdAsync(serviceId);
            if (service == null)
            {
                throw new Exception("Service not found.");
            }

            var workStart = new DateTime(date.Year, date.Month, date.Day, 9, 0, 0);
            var workEnd = new DateTime(date.Year, date.Month, date.Day, 17, 0, 0);

            var startOfDayUtc = date.Date.ToUniversalTime();
            var endOfDayUtc = startOfDayUtc.AddDays(1).AddTicks(-1);

            var employees = await _employeeRepository.GetAllQueryable()
                .Include(x => x.Bookings
                    .Where(x => x.BookingDate >= startOfDayUtc
                           && x.BookingDate <= endOfDayUtc
                           && x.Status != BookingStatus.Cancelled))
                .ThenInclude(b => b.Service)
                .ToListAsync();

            var allAvailableSlots = new List<DateTime>();

            var localTimeZone = TimeZoneInfo.Local;

            foreach (var employee in employees)
            {
                var employeeAvailableSlots = GetAvailableSlotsForEmployee(employee.Bookings, workStart, workEnd, service.Duration, localTimeZone);
                allAvailableSlots.AddRange(employeeAvailableSlots);
            }

            return allAvailableSlots.Select(slot => slot.ToString("HH:mm")).Distinct();
        }

        private List<DateTime> GetAvailableSlotsForEmployee(
            ICollection<BookingEntity> bookings,
            DateTime workStart,
            DateTime workEnd,
            int serviceDuration,
            TimeZoneInfo timeZone)
        {
            var availableSlots = new List<DateTime>();

            var sortedBookings = bookings
                .OrderBy(b => b.BookingDate)
                .ToList();

            var currentSlotStart = workStart;

            foreach (var booking in sortedBookings)
            {
                var bookingDate = TimeZoneInfo.ConvertTimeFromUtc(booking.BookingDate, timeZone);

                while ((bookingDate - currentSlotStart).TotalMinutes >= serviceDuration)
                {
                    availableSlots.Add(currentSlotStart);
                    currentSlotStart = currentSlotStart.AddMinutes(serviceDuration);
                }

                currentSlotStart = bookingDate.AddMinutes(booking.Service.Duration);
            }

            while ((workEnd - currentSlotStart).TotalMinutes >= serviceDuration)
            {
                availableSlots.Add(currentSlotStart);
                currentSlotStart = currentSlotStart.AddMinutes(serviceDuration);
            }

            return availableSlots;
        }
    }
}