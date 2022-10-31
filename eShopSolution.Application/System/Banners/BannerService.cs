using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;

namespace eShopSolution.Application.System.Banners
{
    public class BannerService : IBannerService
    {
        private readonly EShopDbContext _context;

        public BannerService(EShopDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(Banner banner)
        {
            await _context.Banners.AddAsync(banner);

            return await _context.SaveChangesAsync();
        }

        public IQueryable<Banner> GetAll()
        {
            return _context.Banners;
        }

        public async Task<Banner> GetById(int bannerId)
        {
            return await _context.Banners.FindAsync(bannerId);
        }

        public async Task<int> Remove(Banner banner)
        {
            _context.Banners.Remove(banner);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(Banner banner)
        {
            _context.Banners.Update(banner);

            return await _context.SaveChangesAsync();
        }
    }
}
