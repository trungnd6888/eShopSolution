using Microsoft.AspNetCore.Identity;

namespace eShopSolution.Application.System.RoleClaims
{
    public interface IRoleClaimsService
    {
        IQueryable<IdentityRoleClaim<int>> Get();
        Task<int> Create(IdentityRoleClaim<int> roleClaim);
        Task<int> Remove(IdentityRoleClaim<int> roleClaim);
        Task<IdentityRoleClaim<int>?> GetById(int roleClaimId);
    }
}
