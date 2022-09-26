using eShopSolution.Application.Catalog.Distributors;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.Catalog.Distributors;
using eShopSolution.ViewModel.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "DistributorView")]
    public class DistributorsController : ControllerBase
    {
        private readonly IDistributorService _distributorService;

        public DistributorsController(IDistributorService distributorService)
        {
            _distributorService = distributorService;
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] DistributorGetRequest request)
        {
            var query = _distributorService.GetAll();

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.Name.Contains(request.Keyword.Trim()));
            }

            var totalRecord = await query.CountAsync();

            var data = await query.Select(x => new DistributorViewModel()
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();

            var distributors = new PageResult<DistributorViewModel>()
            {
                Data = data,
                TotalRecord = totalRecord,
            };

            return Ok(distributors);
        }

        [HttpGet("{distributorId}")]
        public async Task<ActionResult> GetById(int distributorId)
        {
            var distributor = await _distributorService.GetById(distributorId);

            if (distributor == null) return BadRequest($"Can not find distributor by Id: {distributorId}");

            return Ok(distributor);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] DistributorCreateRequest request)
        {
            Distributor distributor = new Distributor();
            distributor.Name = request.Name;

            var result = await _distributorService.Create(distributor);

            if (result > 0) return Ok("Create distributor success");

            return BadRequest("Fail to create distributor");
        }

        [HttpPatch("{distributorId}")]
        public async Task<ActionResult> Update(int distributorId, [FromBody] DistributorUpdateRequest request)
        {
            var distributor = await _distributorService.GetById(distributorId);

            if (distributor == null) return BadRequest($"Can not find distributor by Id: {distributorId}");

            distributor.Name = request.Name;

            var result = await _distributorService.Update(distributor);

            if (result > 0) return Ok("Update distributor success");

            return BadRequest("Fail to update distributor");
        }

        [HttpDelete("{distributorId}")]
        public async Task<ActionResult> Remove(int distributorId)
        {
            var distributor = await _distributorService.GetById(distributorId);

            if (distributor == null) return BadRequest($"Can not find distributor by Id: {distributorId}");

            var result = await _distributorService.Remove(distributor);

            if (result > 0) return Ok("Remove distributor success");

            return BadRequest("Fail to remove distributor");
        }
    }
}