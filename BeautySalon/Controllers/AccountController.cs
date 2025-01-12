namespace BeautySalon.Controllers
{
    using BeautySalon.Application.Services.Interfaces;
    using BeautySalon.Domain.Models.Requests;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _accountService.LoginAsync(request.Email, request.Password);
            if (!user.Success)
            {
                return Unauthorized(new { message = user.Message });
            }

            return Ok(new { token = user.Token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CustomerRegisterRequest request)
        {
            var result = await _accountService.RegisterCustomerAsync(request);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = "CustomerEntity registered successfully" });
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var currentUser = _accountService.GetCurrentUser(User);

            if (currentUser == null)
            {
                return Unauthorized("User information is not available.");
            }

            return Ok(currentUser);
        }
    }
}
