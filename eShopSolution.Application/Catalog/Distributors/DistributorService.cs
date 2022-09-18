using eShopSolution.Data.EF;
using eShopSolution.ViewModel.Catalog.Distributors;
using Microsoft.EntityFrameworkCore;

namespace eShopSolution.Application.Catalog.Distributors
{
    public class DistributorService : IDistributorService
    {
        private readonly EShopDbContext _context;

        public DistributorService(EShopDbContext context)
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