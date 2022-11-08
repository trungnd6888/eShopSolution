using Microsoft.AspNetCore.Http;

namespace eShopSolution.ViewModel.Catalog.Customers
{
    public class CustomerCreateRequest
    {
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public string? Address { get; set; }
        public string? Tel { get; set; }
        public string? Email { get; set; }
        public List<IFormFile>? ThumbnailImage { get; set; }
    }
}
