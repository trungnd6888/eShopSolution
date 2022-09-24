using eShopSolution.Data.Entities;

namespace eShopSolution.Application.System.Histories
{
    public interface IHistoriesService
    {
        public IQueryable<History> GetAll();
        public Task<History> GetById(int historyId);
        public Task<int> Create(History history);
        public Task<int> Remove(History history);
    }
}
