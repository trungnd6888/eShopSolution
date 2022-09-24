using eShopSolution.Data.Entities;

namespace eShopSolution.Application.System.Users
{
    public interface IUsersService
    {
        Task<int> Create(AppUser user);

        Task<int> Update(AppUser user);
        Task<int> Remove(AppUser user);

        IQueryable<AppUser> GetAll();

        Task<AppUser> GetById(int userId);

        Task<AppUser> GetByUserName(string userCode);
        Task<int> SaveChange();
    }
}
