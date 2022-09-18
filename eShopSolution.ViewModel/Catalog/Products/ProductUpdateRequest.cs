using Microsoft.AspNetCore.Http;

namespace eShopSolution.ViewModel.Catalog.Products
{
    public class ProductUpdateRequest
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string? Detail { get; set; }
        public double Price { get; set; }
        public bool IsApproved { get; set; }
        public int UserId { get; set; }
        public int ApprovedId { get; set; }
        public bool IsNew { get; set; }
        public bool IsBestSale { get; set; }
        public List<int>? Categories { get; set; }
        public List<int>? Distributors { get; set; }
        public List<IFormFile>? ThumbnailImages0 { get; set; }
        public List<IFormFile>? ThumbnailImages1 { get; set; }
        public List<IFormFile>? ThumbnailImages2 { get; set; }
        public List<IFormFile>? ThumbnailImages3 { get; set; }
        public List<IFormFile>? ThumbnailImages4 { get; set; }
        public List<IFormFile>? ThumbnailImages5 { get; set; }
        public List<IFormFile>? ThumbnailImages6 { get; set; }
        public List<IFormFile>? ThumbnailImages7 { get; set; }
        public string? inputHidden0 { get; set; }
        public string? inputHidden1 { get; set; }
        public string? inputHidden2 { get; set; }
        public string? inputHidden3 { get; set; }
        public string? inputHidden4 { get; set; }
        public string? inputHidden5 { get; set; }
        public string? inputHidden6 { get; set; }
        public string? inputHidden7 { get; set; }

    }
}
