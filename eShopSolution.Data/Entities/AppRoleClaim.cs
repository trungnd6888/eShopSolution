using Microsoft.AspNetCore.Identity;

namespace eShopSolution.Data.Entities
{
    public class AppRoleClaim : IdentityRoleClaim<int>
    {
        public AppRole? AppRole { get; set; }
    }
}
