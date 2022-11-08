using eShopSolution.Data.Entities;

namespace eShopSolution.Application.Catalog.Newses
{
    public interface INewsService
    {
        IQueryable<News> GetAll();

        Task<News> GetById(int newsId);

        Task<int> Create(News news);

        Task<int> Update(News news);

        Task<int> Remove(News news);
    }
}
