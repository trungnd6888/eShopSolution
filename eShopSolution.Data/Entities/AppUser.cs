using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Data.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string FullName { get; set; }
        public string AvatarImage { get; set; }
        public List<Product> Products { get; set; }
        public List<News> News { get; set; }
    }
}