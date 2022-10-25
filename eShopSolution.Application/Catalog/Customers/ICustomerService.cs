using eShopSolution.Data.Entities;

namespace eShopSolution.Application.Catalog.Customers
{
    public interface ICustomerService
    {
        IQueryable<Customer> GetAll();

        Task<Customer> GetById(int customerId);

        Task<int> Create(Customer customer);

        Task<int> Update(Customer customer);

        Task<int> Remove(Customer customer);
    }
}
