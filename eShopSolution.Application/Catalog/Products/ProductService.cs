using eShopSolution.Application.Common.FileStorage;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Exceptions;
using eShopSolution.ViewModel.Catalog.ProductImages;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace eShopSolution.Application.Catalog.Products
{
    public class ProductService : IProductService
    {
        private readonly EShopDbContext _context;
        private readonly IStorageService _storageService;

        public ProductService(EShopDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<int> Create(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task<int> Update(Product product)
        {
            _context.Update(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<Product> GetById(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        public async Task<int> UpdatePrice(int productId, double newPrice)
        {
            Product product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Can not find a product by id: {productId}");

            product.Price = newPrice;

            _context.Update(product);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> Remove(Product product)
        {
            _context.Products.Remove(product);

            return await _context.SaveChangesAsync();
        }

        public IQueryable<Product> GetAll()
        {
            return _context.Products;
        }

        public async Task<Product> GetByCode(string productCode)
        {
            var query = _context.Products.Where(x => x.Code.ToUpper().Trim() == productCode.ToUpper().Trim());

            if (await query.CountAsync() == 0) return null;

            var product = await query.FirstAsync();

            return product;
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
        public async Task<int> SaveChange()
        {
            return await _context.SaveChangesAsync();
        }
    }
}