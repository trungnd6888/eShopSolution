using eShopSolution.Application.Catalog.Products;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.Catalog.ProductImages;
using eShopSolution.ViewModel.Catalog.Products;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IManageProductService _manageProductService;

        public ProductsController(IManageProductService manageProductService)
        {
            _manageProductService = manageProductService;
        }

        //http://localhost:port/Products?PageIndex=1&PageSize=10&Keyword=abc&CategoryIds=1&CategoryIds=2
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetManageProductPagingRequest request)
        {
            var products = await _manageProductService.GetAllPaging(request);

            return Ok(products);
        }

        //http://localhost:port/Products/1
        [HttpGet("{productId}")]
        public async Task<ActionResult> GetById(int productId)
        {
            var product = await _manageProductService.GetById(productId);

            if (product == null) return BadRequest("Cannot find product");

            return Ok(product);
        }

        //http://localhost:port/Products
        [HttpPost]
        public async Task<ActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var productId = await _manageProductService.Create(request);
            if (productId == 0) return BadRequest();

            var product = await _manageProductService.GetById(productId);

            return CreatedAtAction(nameof(GetById), new { id = productId }, product);
        }

        //http://localhost:port/Products
        [HttpPut]
        public async Task<ActionResult> Update([FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var affectedResult = await _manageProductService.Update(request);
            if (affectedResult == 0) return BadRequest();

            return Ok();
        }

        //http://localhost:port/Products/1/5000
        [HttpPatch("{productId}/{newPrice}")]
        public async Task<ActionResult> UpdatePrice(int productId, double newPrice)
        {
            var affectedResult = await _manageProductService.UpdatePrice(productId, newPrice);
            if (affectedResult == 0) return BadRequest();

            return Ok();
        }

        //http://localhost:port/Products/1
        [HttpDelete("{productId}")]
        public async Task<ActionResult> Remove(int productId)
        {
            var affectedResult = await _manageProductService.Remove(productId);
            if (affectedResult == 0) return BadRequest();

            return Ok();
        }

        [HttpPost("{productId}/images")]
        public async Task<ActionResult> CreateImage(int productId, ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            int imageId = await _manageProductService.AddImage(productId, request);

            if (imageId == 0) return BadRequest();

            var image = await _manageProductService.GetImageById(imageId);

            return CreatedAtAction(nameof(GetImageById), new { Id = imageId }, image);
        }

        [HttpPut("{productId}/images/{imageId}")]
        public async Task<ActionResult> UpdateImage(int imageId, [FromForm] ProductImageUpdateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var affectedResult = await _manageProductService.UpdateImage(imageId, request);
            if (affectedResult == 0) return BadRequest();

            return Ok();
        }

        [HttpDelete("{productId}/images/{imageId}")]
        public async Task<ActionResult> RemoveImage(int imageId)
        {
            var affectedResult = await _manageProductService.RemoveImage(imageId);
            if (affectedResult == 0) return BadRequest();

            return Ok();
        }

        [HttpGet("{productId}/images/{imageId}")]
        public async Task<ActionResult> GetImageById(int imageId)
        {
            var image = await _manageProductService.GetImageById(imageId);

            return Ok(image);
        }
    }
}