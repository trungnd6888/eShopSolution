namespace eShopSolution.Data.Entities
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Summary { get; set; }
        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsApproved { get; set; }
        public int? UserId { get; set; }
        public int? ApprovedId { get; set; }
        public AppUser? AppUser { get; set; }
    }
}
