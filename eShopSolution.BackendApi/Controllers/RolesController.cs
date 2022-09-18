using eShopSolution.Application.System.Roles;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Exceptions;
using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.System.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] RoleGetRequest request)
        {
            var query = _roleService.GetAll();

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.Name.Contains(request.Keyword.Trim()) || x.Description.Contains(request.Keyword.Trim()));
            }

            //paging
            int totalRecord = await query.CountAsync();

            var data = await query.ToListAsync();

            var roles = new PageResult<AppRole>()
            {
                Data = data,
                TotalRecord = totalRecord
            };

            return Ok(roles);
        }

        [HttpGet("{roleId}")]
        public async Task<ActionResult> GetById(int roleId)
        {
            var role = await _roleService.GetById(roleId);
            if (role == null) return BadRequest("Cannot find product");

            return Ok(role);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] RoleCreateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //create product
            var role = new AppRole()
            {
                Name = request.Name,
                Description = request.Description,
            };

            var roleId = await _roleService.Create(role);
            if (roleId == 0) return BadRequest("Fail to add product");

            return Ok("Success to add product");
        }

        //http://localhost:port/Roles/1
        [HttpPatch("{roleId}")]
        public async Task<ActionResult> Update(int roleId, [FromBody] RoleUpdateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //get old product
            AppRole? role = await _roleService.GetById(roleId);
            if (role == null) throw new EShopException($"Can not find a product by id: {roleId}");

            /*update new product*/
            role.Name = request.Name;
            role.Description = request.Description;

            var result = await _roleService.Update(role);
            if (result == 0) return BadRequest();

            return Ok();
        }

        //http://localhost:port/Roles/1
        [HttpDelete("{roleId}")]
        public async Task<ActionResult> Remove(int roleId)
        {
            var role = await _roleService.GetById(roleId);
            if (role == null) return BadRequest($"Can not find a product by id: {roleId}");

            var result = await _roleService.Remove(role);
            if (result == 0) return BadRequest("Fail to remove product");

            return Ok("Remove product success");
        }
    }
}
