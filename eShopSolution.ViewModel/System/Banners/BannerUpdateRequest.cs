using Microsoft.AspNetCore.Http;

namespace eShopSolution.ViewModel.System.Banners
{
    public class BannerUpdateRequest
    {
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public List<IFormFile>? ThumbnailImage { get; set; }
        public bool IsApproved { get; set; }
        public int Order { get; set; }
        public string? InputHidden { get; set; }
    }
}
