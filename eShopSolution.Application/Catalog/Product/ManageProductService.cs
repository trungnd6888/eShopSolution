using eShopSolution.Application.Common;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Exceptions;
using eShopSolution.ViewModel.Catalog.ProductImages;
using eShopSolution.ViewModel.Catalog.Products;
using eShopSolution.ViewModel.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace eShopSolution.Application.Catalog.Products
{
    public class ManageProductService : IManageProductService
    {
        private readonly EShopDbContext _context;
        private readonly IStorageService _storageService;

        public ManageProductService(EShopDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Product()
            {
                Name = request.Name,
                Code = request.Code,
                Detail = request.Detail,
                Price = request.Price,
                CreateDate = DateTime.Now,
                ApprovedId = request.ApprovedId,
                UserId = request.UserId
            };

            //Save image
            if (request.ThumbnailImage != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage
                    {
                        Caption = "Thumbnail",
                        CreateDate = DateTime.Now,
                        ProductId = request.Id,
                        ImageUrl = await this.SaveFile(request.ThumbnailImage),
                    }
                };
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product.Id;
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            Product product = await _context.Products.FindAsync(request.Id);
            if (product == null) throw new EShopException($"Can not find a product by id: {request.Id}");

            product.Name = request.Name;
            product.Code = request.Code;
            product.Detail = request.Detail;
            product.IsApproved = request.IsApproved;
            product.ApprovedId = request.ApprovedId;
            product.IsNew = request.IsNew;
            product.IsBestSale = request.IsBestSale;

            //Save image
            if (request.ThumbnailImage != null)
            {
                var thumbnailImage = await _context.ProductImages.FirstOrDefaultAsync(x => x.ProductId == product.Id);

                if (thumbnailImage != null)
                {
                    thumbnailImage.ImageUrl = await this.SaveFile(request.ThumbnailImage);

                    _context.ProductImages.Update(thumbnailImage);
                }
            }

            _context.Update(product);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdatePrice(int productId, double newPrice)
        {
            Product product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Can not find a product by id: {productId}");

            product.Price = newPrice;

            _context.Update(product);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> Remove(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Can not find a product by id: {productId}");

            //delete image
            var thumbnaiImages = _context.ProductImages.Where(x => x.ProductId == productId);

            if (thumbnaiImages.Count() > 0)
            {
                foreach (var thumbnaiImage in thumbnaiImages)
                {
                    await _storageService.DeleteFileAsync(thumbnaiImage.ImageUrl);
                }
            }

            _context.Products.Remove(product);

            return await _context.SaveChangesAsync();
        }

        public async Task<PageResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request)
        {
            var query = from p in _context.Products
                        join pc in _context.ProductCategories on p.Id equals pc.ProductId
                        select new { p, pc };

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.p.Name.Contains(request.Keyword));
            }

            if (request.CategoryIds?.Count > 0)
            {
                query = query.Where(x => request.CategoryIds.Contains(x.pc.CategoryId));
            }

            //paging
            int totalRecord = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.p.Name,
                    Code = x.p.Code,
                    CreateDate = x.p.CreateDate,
                    Detail = x.p.Detail,
                    IsApproved = x.p.IsApproved,
                    IsBestSale = x.p.IsBestSale,
                    IsNew = x.p.IsNew,
                    Price = x.p.Price,
                    UserId = x.p.UserId,
                    ApprovedId = x.p.ApprovedId
                }).ToListAsync();

            var pageResult = new PageResult<ProductViewModel>()
            {
                Items = data,
                TotalRecord = totalRecord
            };

            return pageResult;
        }

        public async Task<ProductViewModel> GetById(int productId)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product == null) throw new EShopException("Cannot find a product");

            var data = new ProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Code = product.Code,
                Detail = product.Detail,
                CreateDate = product.CreateDate,
                ApprovedId = product.ApprovedId,
                IsApproved = product.IsApproved,
                UserId = product.UserId,
                Price = product.Price,
                IsBestSale = product.IsBestSale,
                IsNew = product.IsNew
            };

            return data;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

        public async Task<int> AddImage(int productId, ProductImageCreateRequest request)
        {
            var productImage = new ProductImage()
            {
                Caption = request.Caption,
                CreateDate = DateTime.Now,
                ProductId = productId,
                SortOrder = request.SortOrder
            };

            if (request.ImageFile != null)
            {
                productImage.ImageUrl = await this.SaveFile(request.ImageFile);
            }

            _context.ProductImages.Add(productImage);
            return productImage.Id;
        }

        public async Task<int> RemoveImage(int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);

            if (productImage == null) throw new EShopException("Cannot find a product image");

            _context.ProductImages.Remove(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);

            if (productImage == null) throw new EShopException("Cannot find a product");

            productImage.Caption = request.Caption;
            productImage.SortOrder = request.SortOrder;

            if (request.ImageFile != null)
            {
                productImage.ImageUrl = await this.SaveFile(request.ImageFile);
            }

            _context.ProductImages.Update(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<ProductImageViewModel>> GetListImages(int productId)
        {
            var productImages = await _context.ProductImages
                .Where(i => i.ProductId == productId)
                .Select(x => new ProductImageViewModel
                {
                    Id = x.Id,
                    Caption = x.Caption,
                    CreateDate = x.CreateDate,
                    ImageUrl = x.ImageUrl,
                    ProductId = x.ProductId,
                    SortOrder = x.SortOrder
                }).ToListAsync();

            return productImages;
        }

        public async Task<ProductImageViewModel> GetImageById(int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);

            if (productImage == null) throw new EShopException("Cannot find a image");

            var data = new ProductImageViewModel()
            {
                Caption = productImage.Caption,
                CreateDate = productImage.CreateDate,
                Id = productImage.Id,
                ImageUrl = productImage.ImageUrl,
                ProductId = productImage.ProductId,
                SortOrder = productImage.SortOrder
            };

            return data;
        }
    }
}