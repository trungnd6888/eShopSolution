using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;

namespace eShopSolution.Application.System.Forms
{
    public class FormService : IFormService
    {
        private readonly EShopDbContext _context;
        public FormService(EShopDbContext context)
        {
            _context = context;
        }
        public IQueryable<Form> GetAll()
        {
            return _context.Forms;
        }
    }
}
