using eShopSolution.Data.Entities;

namespace eShopSolution.Application.Catalog.ProductDistributors
{
    public interface IProductDistributorsService
    {
        Task<List<ProductDistributor>> GetByProductId(int productId);
        void RemoveNotSave(ProductDistributor productDistributor);
        Task<int> Add(ProductDistributor productDistributor);
    }
}
