namespace eShopSolution.ViewModel.System.Histories
{
    public class HistoryGetRequest
    {
        public DateTime Time { get; set; }
        public int UserId { get; set; }
        public int FormId { get; set; }
        public int ActionId { get; set; }
    }
}
