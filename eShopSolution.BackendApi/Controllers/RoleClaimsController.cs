using eShopSolution.Application.System.RoleClaims;
using eShopSolution.ViewModel.System.RoleClaims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleClaimsController : ControllerBase
    {
        private readonly IRoleClaimsService _roleClaimsService;
        public RoleClaimsController(IRoleClaimsService roleClaimsService)
        {
            _roleClaimsService = roleClaimsService;
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] RoleClaimGetRequest request)
        {
            var query = _roleClaimsService.Get();

            if (request.RoleId > 0)
            {
                query = query.Where(x => x.RoleId == request.RoleId);
            }

            var data = await query.ToListAsync();

            return Ok(data);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromForm] RoleClaimCreateRequest request)
        {
            IdentityRoleClaim<int> roleClaim = new IdentityRoleClaim<int>();
            roleClaim.RoleId = request.RoleId;
            roleClaim.ClaimType = request.ClaimType;
            roleClaim.ClaimValue = request.ClaimValue;

            var result = await _roleClaimsService.Create(roleClaim);

            if (result > 0) return Ok("RoleClaim add success");
            return BadRequest("Fail to roleClaim add");
        }

        [HttpDelete("roleClaimId")]
        public async Task<ActionResult> Remove(int roleClaimId)
        {
            var roleClaim = await _roleClaimsService.GetById(roleClaimId);

            if (roleClaim == null) return BadRequest($"Can not find role claim by id = {0}");

            var result = await _roleClaimsService.Remove(roleClaim);

            if (result > 0) return Ok("remove role claim success");

            return BadRequest("Fail to remove role claim");
        }

        //[HttpDelete("roleId")]
        //public async Task<ActionResult> RemoveByRoleId(int roleId)
        //{
        //    var query = _roleClaimsService.Get();

        //    if (roleId > 0)
        //    {
        //        query = query.Where(x => x.RoleId == roleId);
        //    }

        //    var data = await query.ToListAsync();

        //    if (data != null && data.Count > 0)
        //    {
        //        foreach (var item in data)
        //        {
        //            await _roleClaimsService.Remove(item);
        //        }
        //    }

        //    return Ok();
        //}
    }
}
