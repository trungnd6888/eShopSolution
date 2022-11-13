using eShopSolution.Data.Entities;

namespace eShopSolution.Application.System.RoleClaims
{
    public interface IRoleClaimService
    {
        IQueryable<AppRoleClaim> Get();
        Task<int> Create(AppRoleClaim roleClaim);
        Task<int> Remove(AppRoleClaim roleClaim);
        Task<AppRoleClaim?> GetById(int roleClaimId);
    }
}
