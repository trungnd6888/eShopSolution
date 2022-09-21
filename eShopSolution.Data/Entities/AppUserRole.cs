using Microsoft.AspNetCore.Identity;

namespace eShopSolution.Data.Entities
{
    public class AppUserRole : IdentityUserRole<int>
    {
        public AppUser? AppUser { get; set; }
        public AppRole? AppRole { get; set; }
    }
}
