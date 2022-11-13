using eShopSolution.Data.Entities;

namespace eShopSolution.Application.Catalog.Statuses
{
    public interface IStatusService
    {
        IQueryable<Status> GetAll();
        Task<int> Create(Status status);
        Task<int> Update(Status status);
        Task<int> Remove(Status status);
        Task<Status> GetById(int statusId);
    }
}
