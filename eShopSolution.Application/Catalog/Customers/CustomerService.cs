using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;

namespace eShopSolution.Application.Catalog.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly EShopDbContext _context;

        public CustomerService(EShopDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(Customer customer)
        {
            _context.Customers.Add(customer);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> Remove(Customer customer)
        {
            _context.Customers.Remove(customer);

            return await _context.SaveChangesAsync();
        }

        public IQueryable<Customer> GetAll()
        {
            return _context.Customers;
        }

        public async Task<Customer> GetById(int customerId)
        {
            return await _context.Customers.FindAsync(customerId);
        }

        public async Task<int> Update(Customer customer)
        {
            _context.Customers.Update(customer);

            return await _context.SaveChangesAsync();
        }
    }
}
