using eShopSolution.Data.EF;
using Action = eShopSolution.Data.Entities.Action;

namespace eShopSolution.Application.System.Actions
{
    public class ActionService : IActionService
    {
        private readonly EShopDbContext _context;
        public ActionService(EShopDbContext context)
        {
            _context = context;
        }
        public IQueryable<Action> GetAll()
        {
            return _context.Actions;
        }
    }
}
