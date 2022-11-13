using eShopSolution.Data.Entities;

namespace eShopSolution.Application.Catalog.ProductCategories
{
    public interface IProductCategoryService
    {
        Task<List<ProductCategory>> GetByProductId(int productId);
        void RemoveNotSave(ProductCategory productCategory);
        Task<int> Add(ProductCategory productCategory);
        IQueryable<ProductCategory> GetAll();
    }
}
