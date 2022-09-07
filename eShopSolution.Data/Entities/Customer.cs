using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Data.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public List<Order> Orders { get; set; }
    }
}