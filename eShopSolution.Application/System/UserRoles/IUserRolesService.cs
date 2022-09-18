using Microsoft.AspNetCore.Identity;

namespace eShopSolution.Application.System.UserRoles
{
    public interface IUserRolesService
    {
        Task<List<IdentityUserRole<int>>> GetByUserId(int userId);
        Task<int> Add(IdentityUserRole<int> userRole);
        void RemoveNotSave(IdentityUserRole<int> userRole);
    }
}
