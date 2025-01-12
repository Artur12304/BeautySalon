namespace BeautySalon.Application.Interfaces
{
    using BeautySalon.Domain.Models;
    using BeautySalon.Infrastructure.Repositories;

    public interface IPriceListRepository : IRepository<PriceListEntity>
    {
        Task<IEnumerable<PriceListEntity>> GetActivePriceListsAsync(DateTime date);
        IQueryable<PriceListEntity> GetAllQueryable();
    }
}