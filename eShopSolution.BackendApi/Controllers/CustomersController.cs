using eShopSolution.Application.Catalog.Customers;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.Catalog.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "CustomerView")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] CustomerGetRequest request)
        {
            var query = _customerService.GetAll();

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.Name.Contains(request.Keyword.Trim()) || x.Email.Contains(request.Keyword.Trim()));
            }

            var customers = await query.ToListAsync();


            return Ok(customers);
        }

        [HttpGet("{customerId}")]
        public async Task<ActionResult> GetById(int customerId)
        {
            var customer = await _customerService.GetById(customerId);

            if (customer == null) return BadRequest($"Can not find customer by id: {customerId}");

            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CustomerCreateRequest request)
        {
            Customer customer = new Customer();

            customer.Name = request.Name;
            customer.Email = request.Email;
            customer.Birthday = request.Birthday;
            customer.Address = request.Address;
            customer.Tel = request.Tel;

            var result = await _customerService.Create(customer);

            if (result > 0) return Ok("Add customer success");

            return BadRequest("Fail to add customer");
        }

        [HttpPatch("{customerId}")]
        public async Task<ActionResult> Update(int customerId, [FromBody] CustomerUpdateRequest request)
        {
            var customer = await _customerService.GetById(customerId);

            if (customer == null) return BadRequest($"Can not find customer by id: {customerId}");

            customer.Name = request.Name;
            customer.Birthday = request.Birthday;
            customer.Address = request.Address;
            customer.Tel = request.Tel;
            customer.Email = request.Email;

            var result = await _customerService.Update(customer);

            if (result > 0) return Ok("Update customer success");

            return BadRequest("Fail to update customer");
        }

        [HttpDelete("{customerId}")]
        public async Task<ActionResult> Remove(int customerId)
        {
            var customer = await _customerService.GetById(customerId);

            if (customer == null) return BadRequest($"Can not find customer by id: {customerId}");

            var result = await _customerService.Remove(customer);

            if (result > 0) return Ok("Remove customer success");

            return BadRequest("Fail to remove customer");
        }
    }
}
