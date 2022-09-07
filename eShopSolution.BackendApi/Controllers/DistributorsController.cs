using eShopSolution.Application.Catalog.Distributors;
using Microsoft.AspNetCore.Mvc;
using eShopSolution.Data.Entities;
using Microsoft.AspNetCore.Authorization;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin,member")]
    public class DistributorsController : ControllerBase
    {
        private readonly IManageDistributorService _manageDistributorService;

        public DistributorsController(IManageDistributorService manageDistributorService)
        {
            _manageDistributorService = manageDistributorService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var distributors = await _manageDistributorService.GetAll();

            return Ok(distributors);
        }
    }
}