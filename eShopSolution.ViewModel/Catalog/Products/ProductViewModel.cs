using eShopSolution.ViewModel.Catalog.ProductImages;

namespace eShopSolution.ViewModel.Catalog.Products
{
    public class ProductViewModel
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
        public string UserName { get; set; }
        public string ApprovedName { get; set; }
        public bool IsNew { get; set; }
        public bool IsBestSale { get; set; }
        public List<int> Categories { get; set; }
        public List<int> Distributors { get; set; }
        public List<ProductImageViewModel> Images { get; set; }
    }
}