namespace BeautySalon.Controllers
{
    using BeautySalon.Application.Services.Interfaces;
    using BeautySalon.Domain.Models.Dtos;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/dashboard")]
    [ApiController]
    [Authorize(Roles = "Administrator")]
    public class DashboardController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IBookingService _bookingService;
        private readonly IServiceService _serviceService;

        public DashboardController(IEmployeeService employeeService, IBookingService bookingService, IServiceService serviceService)
        {
            _employeeService = employeeService;
            _bookingService = bookingService;
            _serviceService = serviceService;
        }

        [HttpGet("stats")]
        public async Task<ActionResult<DashboardStatisticsDto>> GetDashboardStats()
        {
            var employeesCount = await _employeeService.GetEmployeeCountAsync();
            var servicesCount = await _serviceService.GetServiceCountAsync();
            var bookingsCount = await _bookingService.GetBookingCountAsync();

            var stats = new DashboardStatisticsDto
            {
                EmployeesCount = employeesCount,
                ServicesCount = servicesCount,
                BookingsCount = bookingsCount
            };

            return Ok(stats);
        }
    }
}