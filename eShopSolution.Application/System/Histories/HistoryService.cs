using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;

namespace eShopSolution.Application.System.Histories
{
    public class HistoryService : IHistoryService
    {
        private readonly EShopDbContext _context;
        public HistoryService(EShopDbContext context)
        {
            _context = context;
        }

        public IQueryable<History> GetAll()
        {
            return _context.Histories;
        }
        public async Task<History> GetById(int historyId)
        {
            return await _context.Histories.FindAsync(historyId);
        }

        public async Task<int> Create(History history)
        {
            _context.Histories.Add(history);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Remove(History history)
        {
            _context.Histories.Remove(history);
            return await _context.SaveChangesAsync();
        }
    }
}
