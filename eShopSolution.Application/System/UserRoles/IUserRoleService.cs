using eShopSolution.Data.Entities;

namespace eShopSolution.Application.System.UserRoles
{
    public interface IUserRoleService
    {
        //Task<List<IdentityUserRole<int>>> GetByUserId(int userId);
        //Task<int> Add(IdentityUserRole<int> userRole);
        //void RemoveNotSave(IdentityUserRole<int> userRole);


        Task<List<AppUserRole>> GetByUserId(int userId);
        Task<int> Add(AppUserRole userRole);
        void RemoveNotSave(AppUserRole userRole);
    }
}
