using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;

namespace eShopSolution.Application.Catalog.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly EShopDbContext _context;

        public CategoryService(EShopDbContext context)
        {
            _context = context;
        }

        public IQueryable<Category> GetAll()
        {
            return _context.Categories;
        }

        public async Task<Category> GetById(int categoryId)
        {
            return await _context.Categories.FindAsync(categoryId);
        }

        public Category GetByIdNoAsync(int categoryId)
        {
            return _context.Categories.Find(categoryId);
        }

        public async Task<int> Create(Category category)
        {
            _context.Categories.Add(category);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Remove(Category category)
        {
            _context.Categories.Remove(category);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(Category category)
        {
            _context.Categories.Update(category);

            return await _context.SaveChangesAsync();
        }
    }
}