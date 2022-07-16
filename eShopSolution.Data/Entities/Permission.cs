using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Data.Entities
{
    public class Permission
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int FunctionId { get; set; }
        public bool Add { get; set; }
        public bool Update { get; set; }
        public bool Remove { get; set; }
        public bool View { get; set; }
    }
}
