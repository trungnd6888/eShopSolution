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
                UserId = request.UserId,
                ApprovedId = request.ApprovedId,
                IsNew = request.IsNew,
                IsBestSale = request.IsBestSale,
                IsApproved = request.IsApproved,
                ProductCategories = new List<ProductCategory>(),
                ProductDistributors = new List<ProductDistributor>(),
                ProductImages = new List<ProductImage>(),
            };

            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            //Add Foreign key
            if (request.Categories != null && request.Categories.Count > 0)
            {
                request.Categories.ForEach(x => product.ProductCategories.Add(
                    new ProductCategory()
                    {
                        ProductId = product.Id,
                        CategoryId = x
                    }));
            }

            if (request.Distributors != null && request.Distributors.Count > 0)
            {
                request.Distributors.ForEach(x => product.ProductDistributors.Add(
                    new ProductDistributor()
                    {
                        ProductId = product.Id,
                        DistributorId = x
                    }));
            }

            //Save image
            if (request.ThumbnailImages0 != null && request.ThumbnailImages0.Count > 0)
            {
                for (int i = 0; i < request.ThumbnailImages0.Count; i++)
                {
                    product.ProductImages.Add(
                          new ProductImage()
                          {
                              Caption = "thumbnail",
                              CreateDate = DateTime.Now,
                              ProductId = product.Id,
                              ImageUrl = await this.SaveFile(request.ThumbnailImages0[i]),
                              SortOrder = 0,
                          });
                }
            }

            if (request.ThumbnailImages1 != null && request.ThumbnailImages1.Count > 0)
            {
                for (int i = 0; i < request.ThumbnailImages1.Count; i++)
                {
                    product.ProductImages.Add(
                          new ProductImage()
                          {
                              Caption = "thumbnail",
                              CreateDate = DateTime.Now,
                              ProductId = product.Id,
                              ImageUrl = await this.SaveFile(request.ThumbnailImages1[i]),
                              SortOrder = 1,
                          });
                }
            }

            if (request.ThumbnailImages2 != null && request.ThumbnailImages2.Count > 0)
            {
                for (int i = 0; i < request.ThumbnailImages2.Count; i++)
                {
                    product.ProductImages.Add(
                          new ProductImage()
                          {
                              Caption = "thumbnail",
                              CreateDate = DateTime.Now,
                              ProductId = product.Id,
                              ImageUrl = await this.SaveFile(request.ThumbnailImages2[i]),
                              SortOrder = 2,
                          });
                }
            }

            if (request.ThumbnailImages3 != null && request.ThumbnailImages3.Count > 0)
            {
                for (int i = 0; i < request.ThumbnailImages3.Count; i++)
                {
                    product.ProductImages.Add(
                          new ProductImage()
                          {
                              Caption = "thumbnail",
                              CreateDate = DateTime.Now,
                              ProductId = product.Id,
                              ImageUrl = await this.SaveFile(request.ThumbnailImages3[i]),
                              SortOrder = 3,
                          });
                }
            }

            if (request.ThumbnailImages4 != null && request.ThumbnailImages4.Count > 0)
            {
                for (int i = 0; i < request.ThumbnailImages4.Count; i++)
                {
                    product.ProductImages.Add(
                          new ProductImage()
                          {
                              Caption = "thumbnail",
                              CreateDate = DateTime.Now,
                              ProductId = product.Id,
                              ImageUrl = await this.SaveFile(request.ThumbnailImages4[i]),
                              SortOrder = 4,
                          });
                }
            }

            if (request.ThumbnailImages5 != null && request.ThumbnailImages5.Count > 0)
            {
                for (int i = 0; i < request.ThumbnailImages5.Count; i++)
                {
                    product.ProductImages.Add(
                          new ProductImage()
                          {
                              Caption = "thumbnail",
                              CreateDate = DateTime.Now,
                              ProductId = product.Id,
                              ImageUrl = await this.SaveFile(request.ThumbnailImages5[i]),
                              SortOrder = 5,
                          });
                }
            }

            if (request.ThumbnailImages6 != null && request.ThumbnailImages6.Count > 0)
            {
                for (int i = 0; i < request.ThumbnailImages6.Count; i++)
                {
                    product.ProductImages.Add(
                          new ProductImage()
                          {
                              Caption = "thumbnail",
                              CreateDate = DateTime.Now,
                              ProductId = product.Id,
                              ImageUrl = await this.SaveFile(request.ThumbnailImages6[i]),
                              SortOrder = 6,
                          });
                }
            }

            if (request.ThumbnailImages7 != null && request.ThumbnailImages7.Count > 0)
            {
                for (int i = 0; i < request.ThumbnailImages7.Count; i++)
                {
                    product.ProductImages.Add(
                          new ProductImage()
                          {
                              Caption = "thumbnail",
                              CreateDate = DateTime.Now,
                              ProductId = product.Id,
                              ImageUrl = await this.SaveFile(request.ThumbnailImages7[i]),
                              SortOrder = 7,
                          });
                }
            }

            await _context.SaveChangesAsync();

            return product.Id;
        }

        public async Task<int> Update(int productId, ProductUpdateRequest request)
        {
            //get old product
            Product? product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Can not find a product by id: {productId}");

            product.ProductCategories = await _context.ProductCategories.Where(x => x.ProductId == productId).ToListAsync();
            product.ProductDistributors = await _context.ProductDistributors.Where(x => x.ProductId == productId).ToListAsync();

            /*update new product*/
            product.Name = request.Name;
            product.Code = request.Code;
            product.Detail = request.Detail;
            product.IsApproved = request.IsApproved;
            product.IsBestSale = request.IsBestSale;
            product.IsNew = request.IsNew;
            product.Price = request.Price;

            //Remove foreign key Old
            foreach (var item in product.ProductCategories)
            {
                _context.Remove(item);
            }

            foreach (var item in product.ProductDistributors)
            {
                _context.Remove(item);
            }

            //Add foreign key New
            foreach (var categoryId in request.Categories)
            {
                product.ProductCategories.Add(new ProductCategory()
                {
                    ProductId = product.Id,
                    CategoryId = categoryId
                });
            }

            foreach (var distributorId in request.Distributors)
            {
                product.ProductDistributors.Add(new ProductDistributor()
                {
                    ProductId = product.Id,
                    DistributorId = distributorId
                });
            }

            //Save image
            //if (request.ThumbnailImage != null)
            //{
            //    var thumbnailImage = await _context.ProductImages.FirstOrDefaultAsync(x => x.ProductId == product.Id);

            //    if (thumbnailImage != null)
            //    {
            //        thumbnailImage.ImageUrl = await this.SaveFile(request.ThumbnailImage);

            //        _context.ProductImages.Update(thumbnailImage);
            //    }
            //}

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

        public async Task<PageResult<ProductViewModel>> GetAll(GetManageProductRequest request)
        {
            var query = from p in _context.Products
                        join pc in _context.ProductCategories on p.Id equals pc.ProductId into table
                        from item in table.DefaultIfEmpty()
                        select new { p, item };

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.p.Name.Contains(request.Keyword));
            }

            if (request.CategoryIds?.Length > 0)
            {
                query = query.Where(x => request.CategoryIds.Contains(x.item.CategoryId));
            }

            var queryGroupBy = query.GroupBy(x => x.p.Id).Select(grp => new
            {
                Id = grp.Key,
                Name = grp.Select(x => x.p.Name).FirstOrDefault(),
                Code = grp.Select(x => x.p.Code).FirstOrDefault(),
                CreateDate = grp.Select(x => x.p.CreateDate).FirstOrDefault(),
                Detail = grp.Select(x => x.p.Detail).FirstOrDefault(),
                IsApproved = grp.Select(x => x.p.IsApproved).FirstOrDefault(),
                IsBestSale = grp.Select(x => x.p.IsBestSale).FirstOrDefault(),
                IsNew = grp.Select(x => x.p.IsNew).FirstOrDefault(),
                Price = grp.Select(x => x.p.Price).FirstOrDefault(),
                UserId = grp.Select(x => x.p.UserId).FirstOrDefault(),
                ApprovedId = grp.Select(x => x.p.ApprovedId).FirstOrDefault(),
            });

            //paging
            int totalRecord = await queryGroupBy.CountAsync();

            //var data = await queryGroupBy.OrderByDescending(x => x.CreateDate).Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
            var data = await queryGroupBy.Select(x => new ProductViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                CreateDate = x.CreateDate,
                Detail = x.Detail,
                IsApproved = x.IsApproved,
                IsBestSale = x.IsBestSale,
                IsNew = x.IsNew,
                Price = x.Price,
                UserId = x.UserId,
                ApprovedId = x.ApprovedId
            }).ToListAsync();

            var pageResult = new PageResult<ProductViewModel>()
            {
                Data = data,
                TotalRecord = totalRecord
            };

            return pageResult;
        }

        public async Task<ProductViewModel> GetById(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EShopException("Cannot find a product");

            var productCategories = await _context.ProductCategories.Where(p => p.ProductId == productId).ToListAsync();
            var productDistributors = await _context.ProductDistributors.Where(p => p.ProductId == productId).ToListAsync();

            List<int> categories = new List<int>();
            foreach (var item in productCategories) categories.Add(item.CategoryId);

            List<int> distributors = new List<int>();
            foreach (var item in productDistributors) distributors.Add(item.DistributorId);

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
                IsNew = product.IsNew,
                Categories = categories,
                Distributors = distributors
            };

            return data;
        }

        public async Task<ProductViewModel> GetByCode(string productCode)
        {
            var query = _context.Products.Where(x => x.Code.ToUpper().Trim() == productCode.ToUpper().Trim());

            if (await query.CountAsync() == 0) return null;

            var product = await query.FirstAsync();

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
            await _context.SaveChangesAsync();
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