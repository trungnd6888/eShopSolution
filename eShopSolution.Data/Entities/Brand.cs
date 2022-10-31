namespace eShopSolution.Data.Entities
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int Order { get; set; }
        public List<Product> Products { get; set; }
    }
}
