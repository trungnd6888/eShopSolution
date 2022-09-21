using Microsoft.AspNetCore.Identity;

namespace eShopSolution.Data.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string FullName { get; set; }
        public string? AvatarImage { get; set; }
        public List<Product> Products { get; set; }
        public List<News> News { get; set; }
        public List<AppUserRole> AppUserRoles { get; set; }
        public List<History> Histories { get; set; }
    }
}