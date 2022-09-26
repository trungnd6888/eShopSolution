using eShopSolution.Data.Entities;

namespace eShopSolution.Application.Catalog.Distributors
{
    public interface IDistributorService
    {
        IQueryable<Distributor> GetAll();

        Task<Distributor> GetById(int distributorId);

        Task<int> Create(Distributor distributor);

        Task<int> Update(Distributor distributor);

        Task<int> Remove(Distributor distributor);


    }
}