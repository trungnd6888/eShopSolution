using eShopSolution.ViewModel.Catalog.OrderDetails;

namespace eShopSolution.ViewModel.Catalog.Orders
{
    public class OrderViewModel
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
        public string? StatusName { get; set; }
        public string? UserName { get; set; }
        public double TotalAmount { get; set; }
        public List<OrderDetailViewModel>? OrderDetails { get; set; }
    }
}
