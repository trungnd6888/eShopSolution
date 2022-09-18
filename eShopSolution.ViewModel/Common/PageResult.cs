using eShopSolution.Data.Entities;

namespace eShopSolution.ViewModel.Common
{
    public class PageResult<T>
    {
        public List<T> Data { set; get; }
        public int TotalRecord { get; set; }

        public static implicit operator PageResult<T>(PageResult<AppRole> v)
        {
            throw new NotImplementedException();
        }
    }
}