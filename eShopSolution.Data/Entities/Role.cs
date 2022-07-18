﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Data.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Describe { get; set; }
        public List<User> Users { get; set; }
        public List<Permission> Permissions { get; set; }
    }
}
