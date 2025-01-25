namespace BeautySalon.Infrastructure.Repositories.PriceList
{
    using BeautySalon.Application.Interfaces;
    using BeautySalon.Domain.Models;
    using BeautySalon.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class PriceListRepository : Repository<PriceListEntity>, IPriceListRepository
    {
        private readonly ApplicationDbContext _context;

        public PriceListRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PriceListEntity>> GetActivePriceListsAsync(DateTime date)
        {
            return await _context.PriceLists
                .Where(pl => pl.StartDate <= date && pl.EndDate >= date)
                .ToListAsync();
        }

        public IQueryable<PriceListEntity> GetAllQueryable()
        {
            return _context.PriceLists.AsQueryable();
        }
    }
}