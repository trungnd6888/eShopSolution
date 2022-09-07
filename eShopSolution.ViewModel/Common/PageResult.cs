namespace eShopSolution.ViewModel.Common
{
    public class PageResult<T>
    {
        public List<T> Data { set; get; }
        public int TotalRecord { get; set; }
    }
}