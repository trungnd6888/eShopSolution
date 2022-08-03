using eShopSolution.Data.EF;
using eShopSolution.ViewModel.Catalog.Products;
using eShopSolution.ViewModel.Common;
using Microsoft.EntityFrameworkCore;

namespace eShopSolution.Application.Catalog.Products
{
    public class PublicProductService : IPublicProductService
    {
        private readonly EShopDbContext _context;

        public PublicProductService(EShopDbContext context)
        {
            _context = context;
        }

        public async Task<PageResult<ProductViewModel>> GetAllByCategoryId(GetPublicProductPagingRequest request)
        {
            var query = from p in _context.Products
                        join pc in _context.ProductCategories on p.Id equals pc.ProductId
                        select new { p, pc };

            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(x => x.pc.CategoryId == request.CategoryId);
            }

            int totalRecord = await query.CountAsync();

            var data = await (query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.p.Name,
                    Code = x.p.Code,
                    Detail = x.p.Detail,
                    Price = x.p.Price,
                    CreateDate = x.p.CreateDate,
                    ApprovedId = x.p.ApprovedId,
                    UserId = x.p.UserId,
                    IsApproved = x.p.IsApproved,
                    IsBestSale = x.p.IsBestSale,
                    IsNew = x.p.IsNew,
                })).ToListAsync();

            var pageResult = new PageResult<ProductViewModel>()
            {
                Items = data,
                TotalRecord = totalRecord
            };

            return pageResult;
        }
    }
}