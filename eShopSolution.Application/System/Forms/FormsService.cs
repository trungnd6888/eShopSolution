using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;

namespace eShopSolution.Application.System.Forms
{
    public class FormsService : IFormsService
    {
        private readonly EShopDbContext _context;
        public FormsService(EShopDbContext context)
        {
            _context = context;
        }
        public IQueryable<Form> GetAll()
        {
            return _context.Forms;
        }
    }
}
