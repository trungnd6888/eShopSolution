using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;

namespace eShopSolution.Application.Catalog.Brands
{
    public class BrandService : IBrandService
    {
        private readonly EShopDbContext _context;

        public BrandService(EShopDbContext context)
        {
            _context = context;
        }

        public IQueryable<Brand> GetAll()
        {
            return _context.Brands;
        }

        public async Task<Brand> GetById(int brandId)
        {
            return await _context.Brands.FindAsync(brandId);
        }

        public async Task<int> Create(Brand brand)
        {
            _context.Brands.Add(brand);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Remove(Brand brand)
        {
            _context.Brands.Remove(brand);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(Brand brand)
        {
            _context.Brands.Update(brand);

            return await _context.SaveChangesAsync();
        }
    }
}
