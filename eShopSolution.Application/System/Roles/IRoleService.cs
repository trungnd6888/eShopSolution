using eShopSolution.Data.Entities;

namespace eShopSolution.Application.System.Roles
{
    public interface IRoleService
    {
        Task<int> Create(AppRole user);

        Task<int> Update(AppRole user);
        Task<int> Remove(AppRole user);

        IQueryable<AppRole> GetAll();

        Task<AppRole> GetById(int roleId);
        Task<int> SaveChange();
    }
}
