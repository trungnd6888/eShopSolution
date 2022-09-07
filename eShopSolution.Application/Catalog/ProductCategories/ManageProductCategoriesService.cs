using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.ProductCategories
{
    public class ManageProductCategoriesService : IManageProductCategoriesService
    {
        private readonly EShopDbContext _context; 
        public ManageProductCategoriesService(EShopDbContext context)
        {
            _context = context;
        }
    }
}
