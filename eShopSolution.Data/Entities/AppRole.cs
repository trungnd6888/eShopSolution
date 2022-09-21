using Microsoft.AspNetCore.Identity;

namespace eShopSolution.Data.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public string Description { get; set; }
        public List<AppUserRole> AppUserRoles { get; set; }
        public List<AppRoleClaim> AppRoleClaims { get; set; }
    }
}
