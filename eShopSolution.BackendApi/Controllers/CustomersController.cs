using eShopSolution.Application.Catalog.Customers;
using eShopSolution.Application.Common.FileStorage;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.Catalog.Customers;
using eShopSolution.ViewModel.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IStorageService _storageService;

        public CustomersController(ICustomerService customerService, IStorageService storageService)
        {
            _customerService = customerService;
            _storageService = storageService;
        }

        [HttpGet]
        [Authorize(Policy = "CustomerView")]
        public async Task<ActionResult> Get([FromQuery] CustomerGetRequest request)
        {
            var query = _customerService.GetAll();

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.Name.Contains(request.Keyword.Trim()) || x.Email.Contains(request.Keyword.Trim()));
            }

            var totalRecord = await query.CountAsync();

            var data = await query.Select(x => new Customer()
            {
                Id = x.Id,
                Name = x.Name,
                Birthday = x.Birthday,
                Address = x.Address,
                ImageUrl = !string.IsNullOrEmpty(x.ImageUrl) ? _storageService.GetFileUrl(x.ImageUrl) : string.Empty,
                Email = x.Email,
                Tel = x.Tel,
            }).ToListAsync();

            var customers = new PageResult<Customer>()
            {
                Data = data,
                TotalRecord = totalRecord,
            };

            return Ok(customers);
        }

        [HttpGet("{customerId}")]
        [Authorize(Policy = "CustomerView")]
        public async Task<ActionResult> GetById(int customerId)
        {
            var customer = await _customerService.GetById(customerId);

            if (customer == null) return BadRequest($"Can not find customer by id: {customerId}");

            customer.ImageUrl = !string.IsNullOrEmpty(customer.ImageUrl) ? _storageService.GetFileUrl(customer.ImageUrl) : string.Empty;

            return Ok(customer);
        }

        [HttpPost]
        [Authorize(Policy = "CustomerView")]
        public async Task<ActionResult> Create([FromForm] CustomerCreateRequest request)
        {
            Customer customer = new Customer();
            customer.Name = request.Name;
            customer.Email = request.Email;
            customer.Birthday = request.Birthday;
            customer.Address = request.Address;
            customer.Tel = request.Tel;

            if (request.ThumbnailImage != null)
            {
                customer.ImageUrl = await this.SaveFile(request.ThumbnailImage[0]);
            }

            var result = await _customerService.Create(customer);

            if (result > 0) return Ok("Add customer success");

            return BadRequest("Fail to add customer");
        }

        [HttpPatch("{customerId}")]
        [Authorize(Policy = "CustomerView")]
        public async Task<ActionResult> Update(int customerId, [FromForm] CustomerUpdateRequest request)
        {
            var customer = await _customerService.GetById(customerId);

            if (customer == null) return BadRequest($"Can not find customer by id: {customerId}");

            customer.Name = request.Name;
            customer.Birthday = request.Birthday;
            customer.Address = request.Address;
            customer.Tel = request.Tel;
            customer.Email = request.Email;

            //remove image 
            if (string.IsNullOrEmpty(request.InputHidden))
            {
                if (!string.IsNullOrEmpty(customer.ImageUrl))
                {
                    await _storageService.DeleteFileAsync(customer.ImageUrl);
                    customer.ImageUrl = null;
                }
            }

            //add image
            if (request.ThumbnailImage != null)
            {
                //remove image after add
                if (!string.IsNullOrEmpty(customer.ImageUrl))
                {
                    await _storageService.DeleteFileAsync(customer.ImageUrl);
                }

                customer.ImageUrl = await this.SaveFile(request.ThumbnailImage[0]);
            }

            var result = await _customerService.Update(customer);

            if (result > 0) return Ok("Update customer success");

            return BadRequest("Fail to update customer");
        }

        [HttpDelete("{customerId}")]
        [Authorize(Policy = "CustomerView")]
        public async Task<ActionResult> Remove(int customerId)
        {
            var customer = await _customerService.GetById(customerId);

            if (customer == null) return BadRequest($"Can not find customer by id: {customerId}");

            var result = await _customerService.Remove(customer);

            if (result > 0) return Ok("Remove customer success");

            return BadRequest("Fail to remove customer");
        }

        [HttpGet("total")]
        [Authorize(Policy = "CustomerView")]
        public async Task<ActionResult> GetTotalAll()
        {
            var customers = _customerService.GetAll();

            var totalRecord = await customers.CountAsync();

            return Ok(totalRecord);
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
    }
}
