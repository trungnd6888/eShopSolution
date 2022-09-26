using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;

namespace eShopSolution.Application.Catalog.Distributors
{
    public class DistributorService : IDistributorService
    {
        private readonly EShopDbContext _context;

        public DistributorService(EShopDbContext context)
        {
            _context = context;
        }

        public IQueryable<Distributor> GetAll()
        {
            return _context.Distributors;
        }

        public async Task<Distributor> GetById(int distributorId)
        {
            return await _context.Distributors.FindAsync(distributorId);
        }

        public async Task<int> Create(Distributor distributor)
        {
            _context.Distributors.Add(distributor);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(Distributor distributor)
        {
            _context.Distributors.Update(distributor);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> Remove(Distributor distributor)
        {
            _context.Distributors.Remove(distributor);

            return await _context.SaveChangesAsync();
        }
    }
}