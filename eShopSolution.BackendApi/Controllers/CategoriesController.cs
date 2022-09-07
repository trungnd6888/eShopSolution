using eShopSolution.Application.Catalog.Categories;
using Microsoft.AspNetCore.Mvc;
using eShopSolution.Data.Entities;
using Microsoft.AspNetCore.Authorization;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin,member")]
    public class CategoriesController : ControllerBase
    {
        private readonly IManageCategoryService _manageCategoryService;

        public CategoriesController(IManageCategoryService manageCategoryService)
        {
            _manageCategoryService = manageCategoryService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var categories = await _manageCategoryService.GetAll();

            return Ok(categories);
        }
    }
}