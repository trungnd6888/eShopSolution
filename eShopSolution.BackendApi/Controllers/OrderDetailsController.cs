using eShopSolution.Application.Catalog.OrderDetails;
using eShopSolution.Application.Catalog.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;
        private readonly IProductService _productService;

        public OrderDetailsController(IOrderDetailService orderDetailService, IProductService productService)
        {
            _orderDetailService = orderDetailService;
            _productService = productService;
        }

        [HttpGet("total")]
        [Authorize(Policy = "OrderView")]
        public async Task<ActionResult> GetTotalPrice()
        {
            var query = from od in _orderDetailService.GetAll()
                        join p in _productService.GetAll() on od.ProductId equals p.Id
                        select new { od, p };

            var totalPrice = await query.SumAsync(x => x.od.Quantity * x.p.Price);

            return Ok(totalPrice);
        }

        [HttpGet("totalquantity")]
        [Authorize(Policy = "OrderView")]
        public async Task<ActionResult> GetTotalQuantity()
        {
            var query = from od in _orderDetailService.GetAll()
                        join p in _productService.GetAll() on od.ProductId equals p.Id
                        select new
                        {
                            ProductId = od.ProductId,
                            ProductCode = p.Code,
                            Quantity = od.Quantity
                        };

            var totalQuantitys = await query
                    .GroupBy(x => x.ProductId)
                    .Select(x => new
                    {
                        ProductId = x.Key,
                        ProductCode = x.Max(y => y.ProductCode),
                        SumQuantity = x.Sum(y => y.Quantity)
                    }).OrderByDescending(x => x.SumQuantity).Take(10).ToListAsync();

            return Ok(totalQuantitys);
        }
    }
}
