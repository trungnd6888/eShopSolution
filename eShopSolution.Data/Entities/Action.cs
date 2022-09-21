namespace eShopSolution.Data.Entities
{
    public class Action
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<History>? Histories { get; set; }
    }
}
