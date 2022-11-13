using eShopSolution.Data.Entities;

namespace eShopSolution.ViewModel.Catalog.Orders
{
    public class OrderCreateRequest
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Tel { get; set; }
        public string? Address { get; set; }
        public int? StatusId { get; set; }
        public int? UserId { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }
    }
}
