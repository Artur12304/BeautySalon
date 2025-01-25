namespace BeautySalon.Controllers
{
    using BeautySalon.Application.Services.Interfaces;
    using BeautySalon.Domain.Models;
    using BeautySalon.Domain.Models.Requests;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServicesController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ServiceEntity>>> GetServices()
        {
            var services = await _serviceService.GetServicesAsync();
            return Ok(services);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ServiceEntity>> GetServiceById(Guid id)
        {
            var service = await _serviceService.GetServiceByIdAsync(id);
            if (service == null)
            {
                return NotFound("Service not found");
            }
            return Ok(service);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator,Employee")]
        [AllowAnonymous]
        public async Task<ActionResult<ServiceEntity>> CreateService(CreateServiceRequest serviceRequest)
        {
            var createdService = await _serviceService.CreateServiceAsync(serviceRequest);
            return CreatedAtAction(nameof(GetServices), new { id = createdService.Id }, createdService);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator,Employee")]
        public async Task<ActionResult<ServiceEntity>> UpdateService(Guid id, ServiceEntity updatedService)
        {
            var service = await _serviceService.UpdateServiceAsync(id, updatedService);
            if (service == null)
            {
                return NotFound("Service not found");
            }

            return Ok(service);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator,Employee")]
        public async Task<IActionResult> DeleteService(Guid id)
        {
            var deleted = await _serviceService.DeleteServiceAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}