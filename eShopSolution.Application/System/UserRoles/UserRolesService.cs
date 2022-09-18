using eShopSolution.Data.EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eShopSolution.Application.System.UserRoles
{
    public class UserRolesService : IUserRolesService
    {
        private readonly EShopDbContext _context;
        public UserRolesService(EShopDbContext context)
        {
            _context = context;
        }

        public async Task<int> Add(IdentityUserRole<int> userRole)
        {
            _context.UserRoles.Add(userRole);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<IdentityUserRole<int>>> GetByUserId(int userId)
        {
            return await _context.UserRoles.Where(x => x.UserId == userId).ToListAsync();
        }

        public void RemoveNotSave(IdentityUserRole<int> userRole)
        {
            _context.UserRoles.Remove(userRole);
        }
    }
}
