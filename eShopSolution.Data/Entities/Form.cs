namespace eShopSolution.Data.Entities
{
    public class Form
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<History>? Histories { get; set; }
    }
}
