using eShopSolution.ViewModel.Catalog.Categories;

namespace eShopSolution.Application.Catalog.Categories
{
    public interface ICategoryService
    {
        Task<List<CategoryViewModel>> GetAll();
    }
}