using eShopSolution.ViewModel.Catalog.ProductImages;
using eShopSolution.ViewModel.Catalog.Products;
using eShopSolution.ViewModel.Common;

namespace eShopSolution.Application.Catalog.Products
{
    public interface IManageProductService
    {
        Task<int> Create(ProductCreateRequest request);

        Task<int> Update(int productId, ProductUpdateRequest request);

        Task<int> UpdatePrice(int productId, double newPrice);

        Task<int> Remove(int productId);

        Task<PageResult<ProductViewModel>> GetAll(GetManageProductRequest request);

        Task<ProductViewModel> GetById(int productId);

        Task<ProductViewModel> GetByCode(string productCode);

        Task<int> AddImage(int productId, ProductImageCreateRequest request);

        Task<int> RemoveImage(int imageId);

        Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request);

        Task<ProductImageViewModel> GetImageById(int imageId);

        Task<List<ProductImageViewModel>> GetListImages(int productId);
    }
}