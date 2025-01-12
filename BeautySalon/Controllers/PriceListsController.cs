namespace BeautySalon.Controllers
{
    using BeautySalon.Application.Services.Interfaces;
    using BeautySalon.Domain.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class PriceListsController : ControllerBase
    {
        private readonly IPriceListService _priceListService;

        public PriceListsController(IPriceListService priceListService)
        {
            _priceListService = priceListService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PriceListEntity>>> GetPriceLists()
        {
            var priceLists = await _priceListService.GetPriceListsAsync();
            return Ok(priceLists);
        }

        [HttpPost]
        [Authorize(Policy = "Administrator,Employee")]
        public async Task<ActionResult> CreatePriceList(PriceListEntity priceList)
        {
            await _priceListService.CreatePriceListAsync(priceList);
            return CreatedAtAction(nameof(GetPriceLists), new { id = priceList.Id }, priceList);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator,Employee")]
        public async Task<IActionResult> DeletePriceList(Guid id)
        {
            var result = await _priceListService.DeletePriceListAsync(id);

            if (!result)
            {
                return NotFound($"PriceList with ID {id} not found.");
            }

            return NoContent();
        }
    }
}
