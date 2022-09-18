using eShopSolution.ViewModel.Catalog.Distributors;

namespace eShopSolution.Application.Catalog.Distributors
{
    public interface IDistributorService
    {
        Task<List<DistributorViewModel>> GetAll();
    }
}