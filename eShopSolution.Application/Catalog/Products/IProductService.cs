using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.Catalog.ProductImages;

namespace eShopSolution.Application.Catalog.Products
{
    public interface IProductService
    {
        Task<int> Create(Product product);

        Task<int> Update(Product product);
        Task<int> Remove(Product product);

        Task<int> UpdatePrice(int productId, double newPrice);

        IQueryable<Product> GetAll();

        Task<Product> GetById(int productId);

        Task<Product> GetByCode(string productCode);

        Task<int> AddImage(int productId, ProductImageCreateRequest request);

        Task<int> RemoveImage(int imageId);

        Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request);

        Task<ProductImageViewModel> GetImageById(int imageId);

        Task<List<ProductImageViewModel>> GetListImages(int productId);
        Task<int> SaveChange();
    }
}