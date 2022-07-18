using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreateDate { get; set; }
        public int RoleId { get; set; }
        public List<Product> Products { get; set; }
        public Role Role { get; set; }
        public List<News> News { get; set; }
    }
}
