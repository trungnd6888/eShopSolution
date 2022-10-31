using Microsoft.AspNetCore.Http;

namespace eShopSolution.ViewModel.System.Banners
{
    public class BannerCreateRequest
    {
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public List<IFormFile>? ThumbnailImage { get; set; }
        public bool IsApproved { get; set; }
        public int Order { get; set; }

    }
}
