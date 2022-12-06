using eShopSolution.Application.Catalog.OrderDetails;
using eShopSolution.Application.Catalog.Orders;
using eShopSolution.Application.Catalog.Products;
using eShopSolution.Application.Catalog.Statuses;
using eShopSolution.Application.Common.Mail;
using eShopSolution.Application.System.Users;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Contants;
using eShopSolution.ViewModel.Catalog.OrderDetails;
using eShopSolution.ViewModel.Catalog.Orders;
using eShopSolution.ViewModel.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;
        private readonly IOrderService _orderService;
        private readonly IStatusService _statusService;
        private readonly IProductService _productService;
        private readonly IMailService _mailService;
        private readonly IUserService _userService;
        public OrdersController(IOrderService orderService, IOrderDetailService orderDetailService, IStatusService statusService, IProductService productService, IMailService mailService, IUserService userService)
        {
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _statusService = statusService;
            _productService = productService;
            _mailService = mailService;
            _userService = userService;
        }

        [HttpGet("/api/public/[controller]")]
        public async Task<ActionResult> GetPublic([FromQuery] OrderGetRequest request)
        {
            var query = from o in _orderService.GetAll()
                        join s in _statusService.GetAll() on o.StatusId equals s.Id
                        into tableS
                        from itemS in tableS.DefaultIfEmpty()
                        join u in _userService.GetAll() on o.UserId equals u.Id
                        into tableU
                        from itemU in tableU.DefaultIfEmpty()
                        select new { o, itemS, itemU };

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.o.Name.Contains(request.Keyword.Trim())
                || x.o.Email.Contains(request.Keyword.Trim())
                || x.o.Address.Contains(request.Keyword.Trim()));
            }

            //get by userId
            if (request.UserId != null)
            {
                query = query.Where(x => x.o.UserId == request.UserId);
            }

            var totalRecord = await query.CountAsync();

            var data = await query.Select(x => new OrderViewModel()
            {
                Id = x.o.Id,
                Name = x.o.Name,
                Email = x.o.Email,
                Address = x.o.Address,
                Tel = x.o.Tel,
                Code = x.o.Code,
                CreateDate = x.o.CreateDate,
                UserId = x.o.UserId,
                StatusId = x.o.StatusId,
                UserName = x.itemU.UserName,
                StatusName = x.itemS.Name,
                OrderDetails = new List<OrderDetailViewModel>(),
            }).ToListAsync();

            //orderDetails
            foreach (var item in data)
            {
                var queryDetail = from od in _orderDetailService.GetByOrderId(item.Id)
                                  join p in _productService.GetAll() on od.ProductId equals p.Id
                                  select new { od, p };

                var orderDetails = await queryDetail.Select(x => new OrderDetailViewModel()
                {
                    ProductName = x.p.Name,
                    ProductCode = x.p.Code,
                    ProductPrice = x.p.Price,
                    ProductId = x.od.ProductId,
                    OrderId = item.Id,
                    Quantity = x.od.Quantity,
                }).ToListAsync();

                item.OrderDetails = orderDetails;
                item.TotalAmount = orderDetails.Sum(x => x.Quantity * x.ProductPrice);
            }

            var orders = new PageResult<OrderViewModel>()
            {
                Data = data,
                TotalRecord = totalRecord,
            };

            return Ok(orders);
        }

        [HttpPost("/api/public/[controller]")]
        public async Task<ActionResult> CreatePublic([FromBody] OrderCreateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // create and check order code
            var orderCode = getProductCode();
            var orderByCode = await _orderService.GetByCode(orderCode);

            while (orderByCode != null)
            {
                orderCode = getProductCode();
                orderByCode = await _orderService.GetByCode(orderCode);
            }

            var order = new Order()
            {
                Code = orderCode,
                Name = request.Name,
                Email = request.Email,
                Address = request.Address,
                Tel = request.Tel,
                CreateDate = DateTime.Now,
                UserId = request.UserId,
                StatusId = SystemContants.STATUS_DEFAULT,
            };

            await _orderService.Create(order);

            order.OrderDetails = request.OrderDetails?.Select(x => new OrderDetail()
            {
                OrderId = order.Id,
                ProductId = x.ProductId,
                Quantity = x.Quantity,
            }).ToList();

            var result = await _orderService.SaveChange();

            //send email
            if (!string.IsNullOrEmpty(order.Email))
            {
                var contentMail = "<p>Đặt hàng thành công</p>";

                _mailService.SentMail(contentMail, order.Email);
            }

            if (result > 0) return Ok("Create order success");

            return BadRequest("Fail to create order");
        }

        [HttpGet]
        [Authorize(Policy = "OrderView")]
        public async Task<ActionResult> Get([FromQuery] OrderGetRequest request)
        {
            var query = from o in _orderService.GetAll()
                        join s in _statusService.GetAll() on o.StatusId equals s.Id
                        into tableS
                        from itemS in tableS.DefaultIfEmpty()
                        join u in _userService.GetAll() on o.UserId equals u.Id
                        into tableU
                        from itemU in tableU.DefaultIfEmpty()
                        select new { o, itemS, itemU };

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.o.Name.Contains(request.Keyword.Trim())
                || x.o.Email.Contains(request.Keyword.Trim())
                || x.o.Address.Contains(request.Keyword.Trim()));
            }

            var totalRecord = await query.CountAsync();

            var data = await query.Select(x => new OrderViewModel()
            {
                Id = x.o.Id,
                Name = x.o.Name,
                Email = x.o.Email,
                Address = x.o.Address,
                Tel = x.o.Tel,
                Code = x.o.Code,
                CreateDate = x.o.CreateDate,
                UserId = x.o.UserId,
                StatusId = x.o.StatusId,
                UserName = x.itemU.UserName,
                StatusName = x.itemS.Name,
                OrderDetails = new List<OrderDetailViewModel>(),
            }).ToListAsync();

            //orderDetails
            foreach (var item in data)
            {
                var queryDetail = from od in _orderDetailService.GetByOrderId(item.Id)
                                  join p in _productService.GetAll() on od.ProductId equals p.Id
                                  select new { od, p };

                var orderDetails = await queryDetail.Select(x => new OrderDetailViewModel()
                {
                    ProductName = x.p.Name,
                    ProductCode = x.p.Code,
                    ProductPrice = x.p.Price,
                    ProductId = x.od.ProductId,
                    OrderId = item.Id,
                    Quantity = x.od.Quantity,
                }).ToListAsync();

                item.OrderDetails = orderDetails;
                item.TotalAmount = orderDetails.Sum(x => x.Quantity * x.ProductPrice);
            }

            var orders = new PageResult<OrderViewModel>()
            {
                Data = data,
                TotalRecord = totalRecord,
            };

            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        [Authorize(Policy = "OrderView")]
        public async Task<ActionResult> GetById(int orderId)
        {
            var order = await _orderService.GetById(orderId);

            if (order == null) return BadRequest($"Can not find order by id: {orderId}");

            return Ok(order);
        }

        [HttpPost]
        [Authorize(Policy = "OrderView")]
        [Authorize(Policy = "OrderCreate")]
        public async Task<ActionResult> Create([FromBody] OrderCreateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //check order code
            var orderByCode = await _orderService.GetByCode(request.Code);

            if (orderByCode != null)
            {
                ModelState.AddModelError("code", "Code Product invalid");
                return BadRequest(ModelState);
            }

            var order = new Order()
            {
                Code = request.Code,
                Name = request.Name,
                Email = request.Email,
                Address = request.Address,
                Tel = request.Tel,
                CreateDate = DateTime.Now,
                UserId = request.UserId,
                StatusId = SystemContants.STATUS_DEFAULT,
            };

            await _orderService.Create(order);

            order.OrderDetails = request.OrderDetails?.Select(x => new OrderDetail()
            {
                OrderId = order.Id,
                ProductId = x.ProductId,
                Quantity = x.Quantity,
            }).ToList();

            var result = await _orderService.SaveChange();
            if (result > 0) return Ok("Create order success");

            return BadRequest("Fail to create order");
        }

        [HttpPatch("{orderId}")]
        [Authorize(Policy = "OrderView")]
        [Authorize(Policy = "OrderUpdate")]
        public async Task<ActionResult> Update(int orderId, [FromBody] OrderUpdateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var order = await _orderService.GetById(orderId);

            if (order == null) return BadRequest($"Can not find order by id: {orderId}");

            //check order code
            var orderByCode = await _orderService.GetByCode(request.Code);

            if (orderByCode != null && orderByCode.Id != orderId)
            {
                ModelState.AddModelError("code", "Code Product invalid");
                return BadRequest(ModelState);
            }

            order.Code = request.Code;
            order.Name = request.Name;
            order.Address = request.Address;
            order.Tel = request.Tel;
            order.Email = request.Email;
            order.StatusId = request.StatusId;

            //remove OrderDetails old
            //var orderDetails = await _orderDetailService.GetByOrderId(order.Id);

            //if (orderDetails != null && orderDetails.Count > 0)
            //{
            //    orderDetails.ForEach(x => _orderDetailService.RemoveNotSave(x));
            //}

            //update OrderDetails new
            //order.OrderDetails = request.OrderDetails?.Select(x => new OrderDetail()
            //{
            //    OrderId = order.Id,
            //    ProductId = x.ProductId,
            //    Quantity = x.Quantity,
            //}).ToList();

            var result = await _orderService.Update(order);

            if (result > 0) return Ok("Update order success");

            return BadRequest("Fail to update order");
        }

        [HttpDelete("{orderId}")]
        [Authorize(Policy = "OrderView")]
        [Authorize(Policy = "OrderRemove")]
        public async Task<ActionResult> Remove(int orderId)
        {
            var order = await _orderService.GetById(orderId);

            if (order == null) return BadRequest($"Can not find order by id: {orderId}");

            var result = await _orderService.Remove(order);

            if (result > 0) return Ok("Remove order success");

            return BadRequest("Fail to remove order");
        }

        [HttpGet("/api/total/[controller]")]
        [Authorize(Policy = "OrderView")]
        public async Task<ActionResult> GetTotalAll()
        {
            var orders = _orderService.GetAll();

            var totalRecord = await orders.CountAsync();

            return Ok(totalRecord);
        }

        private string getProductCode()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();

            return $"DH{_rdm.Next(_min, _max)}";
        }
    }
}
