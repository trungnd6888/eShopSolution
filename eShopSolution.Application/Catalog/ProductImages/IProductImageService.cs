using eShopSolution.Data.Entities;

namespace eShopSolution.Application.Catalog.ProductImages
{
    public interface IProductImageService
    {
        Task<List<ProductImage>> GetByProductId(int productId);
        IEnumerable<ProductImage> GetByProductIdNoAsync(int productId);
        IQueryable<ProductImage> GetAll();
    }
}
