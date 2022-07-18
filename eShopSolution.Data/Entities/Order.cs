using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public int StatusId { get; set; }
        public int CustomerId { get; set; }
        public Status Status { get; set; }
        public Customer Customer { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
