using eShopSolution.Data.EF;
using Microsoft.AspNetCore.Identity;

namespace eShopSolution.Application.System.RoleClaims
{
    public class RoleClaimsService : IRoleClaimsService
    {
        private readonly EShopDbContext _context;
        public RoleClaimsService(EShopDbContext context)
        {
            _context = context;
        }

        public IQueryable<IdentityRoleClaim<int>> Get()
        {
            return _context.RoleClaims;
        }
        public async Task<int> Create(IdentityRoleClaim<int> roleClaim)
        {
            _context.Add(roleClaim);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> Remove(IdentityRoleClaim<int> roleClaim)
        {
            _context.Remove(roleClaim);
            return await _context.SaveChangesAsync();
        }

        public async Task<IdentityRoleClaim<int>?> GetById(int roleClaimId)
        {
            return await _context.RoleClaims.FindAsync(roleClaimId);
        }
    }
}
