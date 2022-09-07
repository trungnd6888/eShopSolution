using eShopSolution.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eShopSolution.Data.EF;
using Microsoft.EntityFrameworkCore;
using eShopSolution.ViewModel.Catalog.Categories;

namespace eShopSolution.Application.Catalog.Categories
{
    public class ManageCategoryService : IManageCategoryService
    {
        private readonly EShopDbContext _context;

        public ManageCategoryService(EShopDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryViewModel>> GetAll()
        {
            var data = await _context.Categories.Select(x => new CategoryViewModel()
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();

            return data;
        }
    }
}