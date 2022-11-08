using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;

namespace eShopSolution.Application.Catalog.Newses
{
    public class NewsService : INewsService
    {
        private readonly EShopDbContext _context;

        public NewsService(EShopDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(News news)
        {
            _context.News.Add(news);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> Remove(News news)
        {
            _context.News.Remove(news);

            return await _context.SaveChangesAsync();
        }

        public IQueryable<News> GetAll()
        {
            return _context.News;
        }

        public async Task<News> GetById(int newsId)
        {
            return await _context.News.FindAsync(newsId);
        }

        public async Task<int> Update(News news)
        {
            _context.News.Update(news);

            return await _context.SaveChangesAsync();
        }
    }
}
