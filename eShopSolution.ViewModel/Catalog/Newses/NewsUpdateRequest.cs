using Microsoft.AspNetCore.Http;

namespace eShopSolution.ViewModel.Catalog.Newses
{
    public class NewsUpdateRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Summary { get; set; }
        public string? Content { get; set; }
        public bool IsApproved { get; set; }
        public string? InputHidden { get; set; }
        public List<IFormFile>? ThumbnailImage { get; set; }
    }
}
