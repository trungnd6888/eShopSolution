using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Detail { get; set; }
        public double Price { get; set; }
        public int MyProperty { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsApproved { get; set; }
        public int ApprovedId { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public int DistributorId { get; set; }
        public bool IsNew { get; set; }
        public bool IsBestSale { get; set; }
    }
}
