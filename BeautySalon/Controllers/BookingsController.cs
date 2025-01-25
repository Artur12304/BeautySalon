namespace BeautySalon.Controllers
{
    using BeautySalon.Application.Services.Interfaces;
    using BeautySalon.Domain.Enums;
    using BeautySalon.Domain.Models;
    using BeautySalon.Domain.Models.Dtos;
    using BeautySalon.Domain.Models.Requests;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;

    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IEmployeeService _employeeService;
        private readonly ICustomerService _customerService;

        public BookingsController(IBookingService bookingService, IEmployeeService employeeService, ICustomerService customerService)
        {
            _bookingService = bookingService;
            _employeeService = employeeService;
            _customerService = customerService;
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<IEnumerable<CustomerBookingDto>>> GetAdminBookings(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] BookingStatus? status)
        {
            var bookings = await _bookingService.GetAdminBookingsAsync(startDate, endDate, status);
            return Ok(bookings);
        }

        [HttpGet("customer")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<IEnumerable<CustomerBookingDto>>> GetCustomerBookings(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] BookingStatus? status)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (userId == null)
            {
                return Unauthorized("User information is missing.");
            }

            var customer = await _customerService.GetCustomerByUserIdAsync(Guid.Parse(userId));

            if (customer == null)
            {
                return NotFound("Customer not found.");
            }

            var bookings = await _bookingService.GetCustomerBookingsAsync(customer.Id, startDate, endDate, status);

            return Ok(bookings);
        }

        [HttpGet("employee")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<IEnumerable<EmployeeBookingDto>>> GetEmployeeBookings([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] BookingStatus? status)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (userId == null)
            {
                return Unauthorized("User information is missing.");
            }
            var employee = await _employeeService.GetEmployeeByUserIdAsync(Guid.Parse(userId));

            if (employee == null)
            {
                return NotFound("Employee not found.");
            }

            var bookings = await _bookingService.GetEmployeeBookingsAsync(employee.Id, startDate, endDate, status);

            return Ok(bookings);
        }

        [HttpGet("available-slots")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<IEnumerable<string>>> GetAvailableSlots(
            string date, [FromQuery] Guid serviceId)
        {
            DateTime selectedDate;

            if (!DateTime.TryParse(date, out selectedDate))
            {
                return BadRequest("Invalid date format.");
            }

            var availableSlots = await _bookingService.GetAvailableTimeSlotsForServiceAsync(selectedDate, serviceId);

            if (availableSlots.Any())
            {
                return Ok(availableSlots);
            }

            return NotFound("No available slots for this day.");
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Customer,Employee,Administrator")]
        public async Task<ActionResult<BookingEntity>> GetBooking(Guid id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult> CreateBooking([FromBody] CreateBookingRequest createBooking)
        {
            if (string.IsNullOrEmpty(createBooking.ServiceId) || string.IsNullOrEmpty(createBooking.CustomerId))
            {
                return BadRequest("ServiceId or CustomerId is missing.");
            }

            if (!DateTime.TryParse(createBooking.BookingDate, out var bookingDate))
            {
                return BadRequest("Invalid date format.");
            }

            if (!Guid.TryParse(createBooking.ServiceId, out var serviceId))
            {
                return BadRequest("Invalid ServiceId format.");
            }

            if (!Guid.TryParse(createBooking.CustomerId, out var customerId))
            {
                return BadRequest("Invalid CustomerId format.");
            }

            var availableEmployee = await _employeeService.GetAvailableEmployeeAsync(serviceId, bookingDate);

            if (availableEmployee == null)
            {
                return Conflict("No available employees for the selected service and time.");
            }

            var result = await _bookingService.CreateBookingAsync(serviceId, customerId, bookingDate, availableEmployee.Id);

            if (result.Conflict)
            {
                return Conflict(result.Message);
            }

            return CreatedAtAction(nameof(GetBooking), new { id = result.BookingId }, result.BookingId);
        }


        [HttpPut("status")]
        [Authorize(Roles = "Administrator,Employee")]
        public async Task<IActionResult> UpdateBookingStatus(UpdateBookingStatusRequest updateBookingstatus)
        {
            if (updateBookingstatus == null)
            {
                return BadRequest();
            }

            var updated = await _bookingService.UpdateBookingStatusAsync(updateBookingstatus);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator,Employee")]
        public async Task<IActionResult> DeleteBooking(Guid id)
        {
            var deleted = await _bookingService.DeleteBookingAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}