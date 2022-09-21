namespace eShopSolution.Data.Entities
{
    public class History
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public int? UserId { get; set; }
        public int? FormId { get; set; }
        public int? ActionId { get; set; }
        public AppUser? AppUser { get; set; }
        public Action? Action { get; set; }
        public Form? Form { get; set; }
    }
}
