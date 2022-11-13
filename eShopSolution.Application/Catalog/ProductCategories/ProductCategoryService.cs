using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShopSolution.Application.Catalog.ProductCategories
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly EShopDbContext _context;
        public ProductCategoryService(EShopDbContext context)
        {
            _context = context;
        }

        public async Task<int> Add(ProductCategory productCategory)
        {
            _context.ProductCategories.Add(productCategory);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<ProductCategory>> GetByProductId(int productId)
        {
            return await _context.ProductCategories.Where(x => x.ProductId == productId).ToListAsync();
        }

        public IQueryable<ProductCategory> GetAll()
        {
            return _context.ProductCategories;
        }

        public void RemoveNotSave(ProductCategory productCategory)
        {
            _context.Remove(productCategory);
        }
    }
}
