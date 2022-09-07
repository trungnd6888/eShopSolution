using Microsoft.AspNetCore.Http;

namespace eShopSolution.ViewModel.Catalog.Products
{
    public class ProductUpdateRequest
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Detail { get; set; }
        public double Price { get; set; }
        public bool IsApproved { get; set; }
        public int UserId { get; set; }
        public int ApprovedId { get; set; }
        public bool IsNew { get; set; }
        public bool IsBestSale { get; set; }
        public int[] Categories { get; set; }
        public int[] Distributors { get; set; }
        //public IFormFile ThumbnailImage { get; set; }
    }
}
