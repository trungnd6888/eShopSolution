using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;

namespace eShopSolution.Application.Catalog.Statuses
{
    public class StatusService : IStatusService
    {
        private readonly EShopDbContext _context;

        public StatusService(EShopDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(Status status)
        {
            await _context.Status.AddAsync(status);

            return await _context.SaveChangesAsync();
        }

        public IQueryable<Status> GetAll()
        {
            return _context.Status;
        }

        public async Task<Status> GetById(int statusId)
        {
            return await _context.Status.FindAsync(statusId);
        }

        public async Task<int> Remove(Status status)
        {
            _context.Status.Remove(status);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(Status status)
        {
            _context.Status.Update(status);

            return await _context.SaveChangesAsync();
        }
    }
}
