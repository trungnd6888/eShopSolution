namespace eShopSolution.ViewModel.System.Histories
{
    public class HistoryViewModel
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string UserName { get; set; }
        public string FormName { get; set; }
        public string ActionName { get; set; }
    }
}
