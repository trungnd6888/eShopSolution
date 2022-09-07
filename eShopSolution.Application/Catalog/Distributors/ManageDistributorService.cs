using eShopSolution.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eShopSolution.Data.EF;
using Microsoft.EntityFrameworkCore;
using eShopSolution.ViewModel.Catalog.Distributors;

namespace eShopSolution.Application.Catalog.Distributors
{
    public class ManageDistributorService : IManageDistributorService
    {
        private readonly EShopDbContext _context;

        public ManageDistributorService(EShopDbContext context)
        {
            _context = context;
        }

        public async Task<List<DistributorViewModel>> GetAll()
        {
            var data = await _context.Distributors.Select(x => new DistributorViewModel()
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();

            return data;
        }
    }
}