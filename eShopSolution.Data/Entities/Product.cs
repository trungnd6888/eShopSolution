
namespace eShopSolution.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Detail { get; set; }
        public double Price { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsApproved { get; set; }
        public int? ApprovedId { get; set; }
        public int? UserId { get; set; }
        public bool IsNew { get; set; }
        public bool IsBestSale { get; set; }
        public AppUser AppUser { get; set; }
        public List<ProductDistributor> ProductDistributors { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public List<ProductImage> ProductImages { get; set; }
    }
}