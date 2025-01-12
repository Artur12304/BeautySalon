namespace BeautySalon.Application.Services.Interfaces
{
    using BeautySalon.Domain.Enums;
    using BeautySalon.Domain.Models;
    using BeautySalon.Domain.Models.Dtos;
    using BeautySalon.Domain.Models.Requests;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBookingService
    {
        Task<int> GetBookingCountAsync();
        Task<IEnumerable<AdminBookingDto>> GetAdminBookingsAsync(DateTime? startDate, DateTime? endDate, BookingStatus? status);
        Task<IEnumerable<CustomerBookingDto>> GetCustomerBookingsAsync(Guid customerId, DateTime? startDate, DateTime? endDate, BookingStatus? status);
        Task<IEnumerable<EmployeeBookingDto>> GetEmployeeBookingsAsync(Guid employeeId, DateTime? startDate, DateTime? endDate, BookingStatus? status);
        Task<BookingEntity?> GetBookingByIdAsync(Guid id);
        Task<(bool Conflict, string? Message, Guid? BookingId)> CreateBookingAsync(Guid serviceId, Guid customerId, DateTime bookingDate, Guid employeeId);
        Task<bool> UpdateBookingStatusAsync(UpdateBookingStatusRequest updateBookingstatus);
        Task<bool> DeleteBookingAsync(Guid id);
        Task<IEnumerable<string>> GetAvailableTimeSlotsForServiceAsync(DateTime date, Guid serviceId);
    }
}
