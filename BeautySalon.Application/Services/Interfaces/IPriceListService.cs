namespace BeautySalon.Application.Services.Interfaces
{
    using BeautySalon.Domain.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPriceListService
    {
        Task<IEnumerable<PriceListEntity>> GetPriceListsAsync();
        Task<PriceListEntity> CreatePriceListAsync(PriceListEntity priceList);
        Task<bool> DeletePriceListAsync(Guid id);
    }
}
