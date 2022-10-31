using eShopSolution.Data.Entities;

namespace eShopSolution.Application.Catalog.Categories
{
    public interface ICategoryService
    {
        IQueryable<Category> GetAll();

        Task<Category> GetById(int categoryId);

        Category GetByIdNoAsync(int categoryId);

        Task<int> Create(Category category);

        Task<int> Update(Category category);

        Task<int> Remove(Category category);
    }
}