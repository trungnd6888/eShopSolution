using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShopSolution.Application.Catalog.ProductDistributors
{
    public class ProductDistributorService : IProductDistributorService
    {
        private readonly EShopDbContext _context;
        public ProductDistributorService(EShopDbContext context)
        {
            _context = context;
        }

        public async Task<int> Add(ProductDistributor productDistributor)
        {
            _context.Add(productDistributor);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<ProductDistributor>> GetByProductId(int productId)
        {
            return await _context.ProductDistributors.Where(x => x.ProductId == productId).ToListAsync();
        }

        public void RemoveNotSave(ProductDistributor productDistributor)
        {
            _context.Remove(productDistributor);
        }
    }
}
