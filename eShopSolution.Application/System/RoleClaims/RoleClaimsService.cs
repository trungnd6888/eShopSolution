using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;

namespace eShopSolution.Application.System.RoleClaims
{
    public class RoleClaimsService : IRoleClaimsService
    {
        private readonly EShopDbContext _context;
        public RoleClaimsService(EShopDbContext context)
        {
            _context = context;
        }

        public IQueryable<AppRoleClaim> Get()
        {
            return _context.AppRoleClaims;
        }
        public async Task<int> Create(AppRoleClaim roleClaim)
        {
            _context.AppRoleClaims.Add(roleClaim);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> Remove(AppRoleClaim roleClaim)
        {
            _context.AppRoleClaims.Remove(roleClaim);
            return await _context.SaveChangesAsync();
        }
        public async Task<AppRoleClaim?> GetById(int roleClaimId)
        {
            return await _context.AppRoleClaims.FindAsync(roleClaimId);
        }
    }
}
