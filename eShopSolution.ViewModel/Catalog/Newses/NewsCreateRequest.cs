using Microsoft.AspNetCore.Http;

namespace eShopSolution.ViewModel.Catalog.Newses
{
    public class NewsCreateRequest
    {
        public string Title { get; set; }
        public string? Summary { get; set; }
        public string? Content { get; set; }
        public bool IsApproved { get; set; }
        public int? UserId { get; set; }
        public int? ApprovedId { get; set; }
        public List<IFormFile>? ThumbnailImage { get; set; }
    }
}
