namespace eShopSolution.ViewModel.Catalog.Newses
{
    public class NewsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Summary { get; set; }
        public string? Content { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsApproved { get; set; }
        public string? ImageUrl { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string AvatarImage { get; set; }
    }
}
