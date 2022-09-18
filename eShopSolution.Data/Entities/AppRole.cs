using Microsoft.AspNetCore.Identity;

namespace eShopSolution.Data.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public string Description { get; set; }
    }
}
