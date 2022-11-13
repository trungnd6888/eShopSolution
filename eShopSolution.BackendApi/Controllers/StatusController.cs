using eShopSolution.Application.Catalog.Statuses;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.Catalog.Statuses;
using eShopSolution.ViewModel.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IStatusService _statusService;

        public StatusController(IStatusService statusService)
        {
            _statusService = statusService;
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] StatusGetRequest request)
        {
            var query = _statusService.GetAll();

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.Name.Contains(request.Keyword.Trim()));
            }

            var totalRecord = await query.CountAsync();

            var data = await query.ToListAsync();

            var statusList = new PageResult<Status>()
            {
                Data = data,
                TotalRecord = totalRecord,
            };

            return Ok(statusList);
        }

        [HttpGet("{statusId}")]
        public async Task<ActionResult> GetById(int statusId)
        {
            var status = await _statusService.GetById(statusId);

            if (status == null) return BadRequest($"Can not find status by id: {statusId}");

            return Ok(status);
        }

        [HttpDelete("{statusId}")]
        public async Task<ActionResult> Remove(int statusId)
        {
            var status = await _statusService.GetById(statusId);

            if (status == null) return BadRequest($"Can not find status by id: {statusId}");

            var result = await _statusService.Remove(status);

            if (result > 0) return Ok("Remove status success");
            return BadRequest("Fail to remove status");
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] StatusCreateRequest request)
        {
            var status = new Status()
            {
                Name = request.Name,
            };

            var result = await _statusService.Create(status);

            if (result > 0) return Ok("Create status success");

            return BadRequest("Fail to create status");
        }

        [HttpPatch("{statusId}")]
        public async Task<ActionResult> Update(int statusId, [FromBody] StatusUpdateRequest request)
        {
            var status = await _statusService.GetById(statusId);

            if (status == null) return BadRequest($"Can not find status by id: {statusId}");

            status.Name = request.Name;

            var result = await _statusService.Update(status);

            if (result > 0) return Ok("Update status success");

            return BadRequest("Fail to update status");
        }
    }
}
