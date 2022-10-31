namespace eShopSolution.Data.Entities
{
    public class Banner
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsApproved { get; set; }
        public int Order { get; set; }

    }
}
