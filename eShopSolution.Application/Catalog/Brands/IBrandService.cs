using eShopSolution.Data.Entities;

namespace eShopSolution.Application.Catalog.Brands
{
    public interface IBrandService
    {
        IQueryable<Brand> GetAll();

        Task<Brand> GetById(int brandId);

        Task<int> Create(Brand brand);

        Task<int> Update(Brand brand);

        Task<int> Remove(Brand brand);
    }
}
