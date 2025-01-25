namespace BeautySalon.Controllers
{
    using BeautySalon.Application.Services.Interfaces;
    using BeautySalon.Domain.Models.Dtos;
    using BeautySalon.Domain.Models.Requests;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAccountService _accountService;

        public EmployeesController(IEmployeeService employeeService, IAccountService accountService)
        {
            _employeeService = employeeService;
            _accountService = accountService;
        }

        [HttpGet]
        [Authorize(Policy = "Administrator")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
        {
            var employees = await _employeeService.GetEmployeesAsync();
            return Ok(employees);
        }

        [HttpPost]
        [Authorize(Policy = "Administrator")]
        public async Task<ActionResult> CreateEmployee([FromBody] CreateEmployeeRequest createEmployeeRequest)
        {
            var currentUser = _accountService.GetCurrentUser(User);

            if (currentUser == null)
            {
                return Unauthorized("User information is not available.");
            }

            var currentUserId = currentUser.UserId;

            var (success, message) = await _accountService.RegisterEmployeeAsync(createEmployeeRequest, currentUserId);

            if (!success)
            {
                return BadRequest(new { message });
            }

            return Ok(new { message = "Employee created successfully." });
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Administrator")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var deleted = await _employeeService.DeleteEmployeeAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}