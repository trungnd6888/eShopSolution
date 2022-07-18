using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Data.Entities
{
    public class ProductDistributor
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int DistributorId { get; set; }
        public Distributor Distributor { get; set; }
    }
}
