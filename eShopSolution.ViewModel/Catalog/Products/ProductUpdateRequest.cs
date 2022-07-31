using Microsoft.AspNetCore.Http;

namespace eShopSolution.ViewModel.Catalog.Products
{
    public class ProductUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Detail { get; set; }
        public bool IsApproved { get; set; }
        public int ApprovedId { get; set; }
        public bool IsNew { get; set; }
        public bool IsBestSale { get; set; }
        public IFormFile ThumbnailImage { get; set; }
    }
}
