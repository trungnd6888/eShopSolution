using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShopSolution.Application.Catalog.ProductImages
{
    public class ProductImageService : IProductImageService
    {
        private readonly EShopDbContext _context;
        public ProductImageService(EShopDbContext context)
        {
            _context = context;
        }

        public IQueryable<ProductImage> GetAll()
        {
            return _context.ProductImages;
        }

        public async Task<List<ProductImage>> GetByProductId(int productId)
        {
            return await _context.ProductImages.Where(x => x.ProductId == productId).ToListAsync();
        }

        public IEnumerable<ProductImage> GetByProductIdNoAsync(int productId)
        {
            return _context.ProductImages.Where(x => x.ProductId == productId).ToList();
        }
    }
}
