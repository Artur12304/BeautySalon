namespace BeautySalon.Application.Services
{
    using BeautySalon.Application.Services.Interfaces;
    using BeautySalon.Domain.Models;
    using BeautySalon.Infrastructure.Repositories;
    using Microsoft.Extensions.Logging;

    public class PriceListService : IPriceListService
    {
        private readonly IRepository<PriceListEntity> _priceListRepository;
        private readonly ILogger<PriceListService> _logger;

        public PriceListService(IRepository<PriceListEntity> priceListRepository, ILogger<PriceListService> logger)
        {
            _priceListRepository = priceListRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<PriceListEntity>> GetPriceListsAsync()
        {
            _logger.LogInformation("Fetching all price lists");
            return await _priceListRepository.GetAllAsync();
        }

        public async Task<PriceListEntity> CreatePriceListAsync(PriceListEntity priceList)
        {
            _logger.LogInformation("Creating a new price list for ServiceId: {ServiceId}", priceList.ServiceId);
            await _priceListRepository.AddAsync(priceList);
            _logger.LogInformation("Price list created successfully with Id: {Id}", priceList.Id);
            return priceList;
        }

        public async Task<bool> DeletePriceListAsync(Guid id)
        {
            _logger.LogInformation("Attempting to delete price list with Id: {Id}", id);

            var priceList = await _priceListRepository.GetByIdAsync(id);
            if (priceList == null)
            {
                _logger.LogWarning("Failed to delete price list: PriceList not found for Id: {Id}", id);
                return false;
            }

            await _priceListRepository.DeleteAsync(id);
            _logger.LogInformation("Price list deleted successfully with Id: {Id}", id);
            return true;
        }
    }
}