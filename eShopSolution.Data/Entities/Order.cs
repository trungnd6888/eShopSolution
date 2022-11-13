namespace eShopSolution.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime CreateDate { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Tel { get; set; }
        public string? Address { get; set; }
        public int? StatusId { get; set; }
        public int? UserId { get; set; }
        public Status? Status { get; set; }
        public AppUser? AppUser { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }

    }
}
