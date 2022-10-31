using eShopSolution.Data.Entities;

namespace eShopSolution.Application.System.Banners
{
    public interface IBannerService
    {
        IQueryable<Banner> GetAll();

        Task<Banner> GetById(int bannerId);

        Task<int> Create(Banner banner);

        Task<int> Update(Banner banner);

        Task<int> Remove(Banner banner);
    }
}
