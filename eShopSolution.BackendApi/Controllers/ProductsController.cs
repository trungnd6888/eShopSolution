using eShopSolution.Application.Catalog.ProductCategories;
using eShopSolution.Application.Catalog.ProductDistributors;
using eShopSolution.Application.Catalog.ProductImages;
using eShopSolution.Application.Catalog.Products;
using eShopSolution.Application.Common;
using eShopSolution.Application.System.Histories;
using eShopSolution.Application.System.Users;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Exceptions;
using eShopSolution.ViewModel.Catalog.ProductImages;
using eShopSolution.ViewModel.Catalog.Products;
using eShopSolution.ViewModel.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using static eShopSolution.Utilities.Contants.SystemContants;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductsController : ControllerBase
    {
        private readonly IStorageService _storageService;
        private readonly IProductService _productService;
        private readonly IProductImagesService _productImagesService;
        private readonly IProductCategoriesService _productCategoriesService;
        private readonly IProductDistributorsService _productDistributorsService;
        private readonly IHistoriesService _historiesService;
        private readonly IUsersService _usersService;

        public ProductsController(IProductService productService, IProductCategoriesService productCategoriesService,
            IProductDistributorsService productDistributorsService, IProductImagesService productImagesService,
            IStorageService storageService, IHistoriesService historiesService, IUsersService usersService)
        {
            _storageService = storageService;
            _productService = productService;
            _productImagesService = productImagesService;
            _productCategoriesService = productCategoriesService;
            _productDistributorsService = productDistributorsService;
            _historiesService = historiesService;
            _usersService = usersService;
        }

        [HttpGet("/api/public/[controller]")]
        public async Task<ActionResult> GetPublic([FromQuery] ProductGetRequest request)
        {
            var query = from p in _productService.GetAll()
                        join u in _usersService.GetAll() on p.UserId equals u.Id
                        join ua in _usersService.GetAll() on p.ApprovedId equals ua.Id
                        join pc in _productCategoriesService.GetAll() on p.Id equals pc.ProductId
                        into table
                        from item in table.DefaultIfEmpty()
                        select new { p, u, ua, item };

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x =>
                    x.p.Name.Contains(request.Keyword.Trim())
                    || x.p.Code.Contains(request.Keyword.Trim())
                    || x.p.Detail.Contains(request.Keyword.Trim())
                );
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
                UserName = grp.Select(x => x.u.UserName).FirstOrDefault(),
                ApprovedId = grp.Select(x => x.p.ApprovedId).FirstOrDefault(),
                ApprovedName = grp.Select(x => x.ua.UserName).FirstOrDefault(),
            }); ;

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
                UserName = x.UserName,
                ApprovedId = x.ApprovedId,
                ApprovedName = x.ApprovedName,
            }).ToListAsync();

            //Set categories, distributors, Images for data
            foreach (var item in data)
            {
                //categories
                var productCategories = await _productCategoriesService.GetByProductId(item.Id);

                List<int> categories = new List<int>();
                foreach (var i in productCategories) categories.Add(i.CategoryId);

                item.Categories = categories;

                //distributors
                var productDistributors = await _productDistributorsService.GetByProductId(item.Id);

                List<int> distributors = new List<int>();
                foreach (var i in productDistributors) distributors.Add(i.DistributorId);

                item.Distributors = distributors;

                //images
                item.Images = _productImagesService.GetByProductIdNoAsync(item.Id)
                    .Select(x => new ProductImageViewModel()
                    {
                        Id = x.Id,
                        Caption = x.Caption,
                        CreateDate = x.CreateDate,
                        ImageUrl = _storageService.GetFileUrl(x.ImageUrl),
                        ProductId = x.ProductId,
                        SortOrder = x.SortOrder,
                    }).ToList();
            }

            var products = new PageResult<ProductViewModel>()
            {
                Data = data,
                TotalRecord = totalRecord
            };

            return Ok(products);
        }

        [HttpGet("/api/public/[controller]/{productId}")]
        public async Task<ActionResult> GetByIdPublic(int productId)
        {
            var product = await _productService.GetById(productId);
            if (product == null) return BadRequest("Cannot find product");

            var productCategories = await _productCategoriesService.GetByProductId(productId);
            var productDistributors = await _productDistributorsService.GetByProductId(productId);
            var productImages = await _productImagesService.GetByProductId(productId);

            List<int> categories = new List<int>();
            foreach (var item in productCategories) categories.Add(item.CategoryId);

            List<int> distributors = new List<int>();
            foreach (var item in productDistributors) distributors.Add(item.DistributorId);

            List<ProductImageViewModel> images = new List<ProductImageViewModel>();
            foreach (var item in productImages) images.Add(new ProductImageViewModel()
            {
                Id = item.Id,
                ProductId = item.Id,
                ImageUrl = _storageService.GetFileUrl(item.ImageUrl),
                Caption = item.Caption,
                CreateDate = item.CreateDate,
                SortOrder = item.SortOrder,
            });

            var productVM = new ProductViewModel()
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
                Distributors = distributors,
                Images = images,
            };

            return Ok(productVM);
        }

        //http://localhost:port/Products?PageIndex=1&PageSize=10&Keyword=abc&CategoryIds=1&CategoryIds=2
        [HttpGet]
        [Authorize(Policy = "ProductView")]
        public async Task<ActionResult> Get([FromQuery] ProductGetRequest request)
        {
            var query = from p in _productService.GetAll()
                        join u in _usersService.GetAll() on p.UserId equals u.Id
                        join ua in _usersService.GetAll() on p.ApprovedId equals ua.Id
                        join pc in _productCategoriesService.GetAll() on p.Id equals pc.ProductId
                        into table
                        from item in table.DefaultIfEmpty()
                        select new { p, u, ua, item };

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x =>
                    x.p.Name.Contains(request.Keyword.Trim())
                    || x.p.Code.Contains(request.Keyword.Trim())
                    || x.p.Detail.Contains(request.Keyword.Trim())
                );
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
                UserName = grp.Select(x => x.u.UserName).FirstOrDefault(),
                ApprovedId = grp.Select(x => x.p.ApprovedId).FirstOrDefault(),
                ApprovedName = grp.Select(x => x.ua.UserName).FirstOrDefault(),
            }); ;

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
                UserName = x.UserName,
                ApprovedId = x.ApprovedId,
                ApprovedName = x.ApprovedName,
            }).ToListAsync();

            //Set Images for data
            foreach (var item in data)
            {
                item.Images = _productImagesService.GetByProductIdNoAsync(item.Id)
                    .Select(x => new ProductImageViewModel()
                    {
                        Id = x.Id,
                        Caption = x.Caption,
                        CreateDate = x.CreateDate,
                        ImageUrl = _storageService.GetFileUrl(x.ImageUrl),
                        ProductId = x.ProductId,
                        SortOrder = x.SortOrder,
                    }).ToList();
            }

            var products = new PageResult<ProductViewModel>()
            {
                Data = data,
                TotalRecord = totalRecord
            };

            return Ok(products);
        }

        //http://localhost:port/Products/1
        [HttpGet("{productId}")]
        [Authorize(Policy = "ProductView")]
        public async Task<ActionResult> GetById(int productId)
        {
            var product = await _productService.GetById(productId);
            if (product == null) return BadRequest("Cannot find product");

            var productCategories = await _productCategoriesService.GetByProductId(productId);
            var productDistributors = await _productDistributorsService.GetByProductId(productId);
            var productImages = await _productImagesService.GetByProductId(productId);

            List<int> categories = new List<int>();
            foreach (var item in productCategories) categories.Add(item.CategoryId);

            List<int> distributors = new List<int>();
            foreach (var item in productDistributors) distributors.Add(item.DistributorId);

            List<ProductImageViewModel> images = new List<ProductImageViewModel>();
            foreach (var item in productImages) images.Add(new ProductImageViewModel()
            {
                Id = item.Id,
                ProductId = item.Id,
                ImageUrl = _storageService.GetFileUrl(item.ImageUrl),
                Caption = item.Caption,
                CreateDate = item.CreateDate,
                SortOrder = item.SortOrder,
            });

            var productVM = new ProductViewModel()
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
                Distributors = distributors,
                Images = images,
            };

            return Ok(productVM);
        }

        //http://localhost:port/Products
        [HttpPost]
        [Authorize(Policy = "ProductView")]
        [Authorize(Policy = "ProductCreate")]
        public async Task<ActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //Check Code Product
            var productByCode = await _productService.GetByCode(request.Code);

            if (productByCode != null)
            {
                ModelState.AddModelError("code", "Code Product invalid");
                return BadRequest(ModelState);
            }

            //create product
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

            var productId = await _productService.Create(product);
            if (productId == 0) return BadRequest("Fail to add product");

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
            await SaveImageOnAdd(request.ThumbnailImages0, product, (int)SortOrderNumber.ZERO);
            await SaveImageOnAdd(request.ThumbnailImages1, product, (int)SortOrderNumber.FIRST);
            await SaveImageOnAdd(request.ThumbnailImages2, product, (int)SortOrderNumber.SECOND);
            await SaveImageOnAdd(request.ThumbnailImages3, product, (int)SortOrderNumber.THIRD);
            await SaveImageOnAdd(request.ThumbnailImages4, product, (int)SortOrderNumber.FOURTH);
            await SaveImageOnAdd(request.ThumbnailImages5, product, (int)SortOrderNumber.FIFTH);
            await SaveImageOnAdd(request.ThumbnailImages6, product, (int)SortOrderNumber.SIXTH);
            await SaveImageOnAdd(request.ThumbnailImages7, product, (int)SortOrderNumber.SEVENTH);

            await _productService.SaveChange();

            //add history
            History history = new History()
            {
                Time = DateTime.Now,
                ActionId = (int)ActionId.CREATE,
                FormId = (int)FormId.PRODUCT,
                UserId = request.UserId,
            };

            await _historiesService.Create(history);

            return Ok("Success to add product");
        }

        //http://localhost:port/Products/1
        [HttpPatch("{productId}")]
        [Authorize(Policy = "ProductView")]
        [Authorize(Policy = "ProductUpdate")]

        public async Task<ActionResult> Update(int productId, [FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //Check Code Product
            var productByCode = await _productService.GetByCode(request.Code);

            if (productByCode != null && productByCode.Id != productId)
            {
                ModelState.AddModelError("code", "Code Product invalid");
                return BadRequest(ModelState);
            }

            //get old product
            Product? product = await _productService.GetById(productId);
            if (product == null) throw new EShopException($"Can not find a product by id: {productId}");

            product.ProductCategories = await _productCategoriesService.GetByProductId(productId);
            product.ProductDistributors = await _productDistributorsService.GetByProductId(productId);
            product.ProductImages = await _productImagesService.GetByProductId(productId);

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
                _productCategoriesService.RemoveNotSave(item);
            }
            product.ProductCategories = new List<ProductCategory>();

            foreach (var item in product.ProductDistributors)
            {
                _productDistributorsService.RemoveNotSave(item);
            }
            product.ProductDistributors = new List<ProductDistributor>();

            //Add foreign key New
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

            //Remove image old
            await RemoveImageOnUpdate(request.inputHidden0, product, (int)SortOrderNumber.ZERO);
            await RemoveImageOnUpdate(request.inputHidden1, product, (int)SortOrderNumber.FIRST);
            await RemoveImageOnUpdate(request.inputHidden2, product, (int)SortOrderNumber.SECOND);
            await RemoveImageOnUpdate(request.inputHidden3, product, (int)SortOrderNumber.THIRD);
            await RemoveImageOnUpdate(request.inputHidden4, product, (int)SortOrderNumber.FOURTH);
            await RemoveImageOnUpdate(request.inputHidden5, product, (int)SortOrderNumber.FIFTH);
            await RemoveImageOnUpdate(request.inputHidden6, product, (int)SortOrderNumber.SIXTH);
            await RemoveImageOnUpdate(request.inputHidden7, product, (int)SortOrderNumber.SEVENTH);

            //Save image
            SaveImageOnUpdate(request.ThumbnailImages0, product, (int)SortOrderNumber.ZERO);
            await SaveImageOnUpdate(request.ThumbnailImages1, product, (int)SortOrderNumber.FIRST);
            await SaveImageOnUpdate(request.ThumbnailImages2, product, (int)SortOrderNumber.SECOND);
            await SaveImageOnUpdate(request.ThumbnailImages3, product, (int)SortOrderNumber.THIRD);
            await SaveImageOnUpdate(request.ThumbnailImages4, product, (int)SortOrderNumber.FOURTH);
            await SaveImageOnUpdate(request.ThumbnailImages5, product, (int)SortOrderNumber.FIFTH);
            await SaveImageOnUpdate(request.ThumbnailImages6, product, (int)SortOrderNumber.SIXTH);
            await SaveImageOnUpdate(request.ThumbnailImages7, product, (int)SortOrderNumber.SEVENTH);

            var result = await _productService.Update(product);
            if (result == 0) return BadRequest();

            return Ok();
        }

        //http://localhost:port/Products/1
        [HttpDelete("{productId}")]
        [Authorize(Policy = "ProductView")]
        [Authorize(Policy = "ProductRemove")]
        public async Task<ActionResult> Remove(int productId)
        {
            var product = await _productService.GetById(productId);
            if (product == null) return BadRequest($"Can not find a product by id: {productId}");

            //delete image
            var thumbnaiImages = await _productImagesService.GetByProductId(productId);

            if (thumbnaiImages.Count() > 0)
            {
                foreach (var thumbnaiImage in thumbnaiImages)
                {
                    await _storageService.DeleteFileAsync(thumbnaiImage.ImageUrl);
                }
            }

            var result = await _productService.Remove(product);
            if (result == 0) return BadRequest("Fail to remove product");

            return Ok("Remove product success");
        }

        //http://localhost:port/Products/1/5000
        [HttpPatch("{productId}/{newPrice}")]
        [Authorize(Policy = "ProductView")]
        public async Task<ActionResult> UpdatePrice(int productId, double newPrice)
        {
            var affectedResult = await _productService.UpdatePrice(productId, newPrice);
            if (affectedResult == 0) return BadRequest();

            return Ok();
        }

        [HttpPost("{productId}/images")]
        [Authorize(Policy = "ProductView")]
        public async Task<ActionResult> CreateImage(int productId, [FromForm] ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            int imageId = await _productService.AddImage(productId, request);

            if (imageId == 0) return BadRequest();

            var image = await _productService.GetImageById(imageId);

            return Ok(image);
        }

        [HttpPatch("{productId}/images/{imageId}")]
        [Authorize(Policy = "ProductView")]
        public async Task<ActionResult> UpdateImage(int imageId, [FromBody] ProductImageUpdateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var affectedResult = await _productService.UpdateImage(imageId, request);
            if (affectedResult == 0) return BadRequest();

            return Ok();
        }

        [HttpDelete("{productId}/images/{imageId}")]
        [Authorize(Policy = "ProductView")]
        public async Task<ActionResult> RemoveImage(int imageId)
        {
            var affectedResult = await _productService.RemoveImage(imageId);
            if (affectedResult == 0) return BadRequest();

            return Ok();
        }

        [HttpGet("{productId}/images/{imageId}")]
        [Authorize(Policy = "ProductView")]
        public async Task<ActionResult> GetImageById(int imageId)
        {
            var image = await _productService.GetImageById(imageId);

            return Ok(image);
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

        private async Task SaveImageOnAdd(List<IFormFile>? formfiles, Product product, int order)
        {
            if (formfiles != null && formfiles.Count > 0)
            {
                for (int i = 0; i < formfiles.Count; i++)
                {
                    product.ProductImages.Add(
                          new ProductImage()
                          {
                              Caption = "thumbnail",
                              CreateDate = DateTime.Now,
                              ProductId = product.Id,
                              ImageUrl = await this.SaveFile(formfiles[i]),
                              SortOrder = order,
                          });
                }
            }
        }

        private async Task RemoveImageOnUpdate(string? inputHidden, Product product, int order)
        {
            if (string.IsNullOrEmpty(inputHidden))
            {
                var removeList = product.ProductImages.Where(x => x.SortOrder == order).ToList();

                foreach (var item in removeList)
                {
                    await _storageService.DeleteFileAsync(item.ImageUrl);
                    product.ProductImages.Remove(item);
                }
            }
        }

        private async Task SaveImageOnUpdate(List<IFormFile>? formFiles, Product product, int order)
        {
            if (formFiles != null && formFiles.Count > 0)
            {
                //Remove image old before add 
                product.ProductImages.Where(x => x.SortOrder == order).ToList().ForEach(async y =>
                {
                    await _storageService.DeleteFileAsync(y.ImageUrl);
                    product.ProductImages.Remove(y);
                });

                for (int i = 0; i < formFiles.Count; i++)
                {
                    //Add new
                    product.ProductImages.Add(
                          new ProductImage()
                          {
                              Caption = "thumbnail",
                              CreateDate = DateTime.Now,
                              ProductId = product.Id,
                              ImageUrl = await this.SaveFile(formFiles[i]),
                              SortOrder = order,
                          });
                }
            }
        }
    }
}