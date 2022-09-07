using Microsoft.AspNetCore.Http;

namespace eShopSolution.ViewModel.Catalog.ProductImages
{
    public class ProductImageCreateRequest
    {
        public IFormFile ImageFile { get; set; }
        public string Caption { get; set; }
        public int SortOrder { get; set; }
    }
}
