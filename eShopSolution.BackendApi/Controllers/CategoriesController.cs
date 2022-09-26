using eShopSolution.Application.Catalog.Categories;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.Catalog.Categories;
using eShopSolution.ViewModel.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "CategoryView")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] CategoryGetRequest request)
        {
            var query = _categoryService.GetAll();

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.Name.Contains(request.Keyword.Trim()));
            };

            var totalRecord = await query.CountAsync();

            var data = await query.Select(x => new CategoryViewModel()
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();

            var categories = new PageResult<CategoryViewModel>()
            {
                Data = data,
                TotalRecord = totalRecord,
            };

            return Ok(categories);
        }

        [HttpGet("{categoryId}")]
        public async Task<ActionResult> GetById(int categoryId)
        {
            var category = await _categoryService.GetById(categoryId);

            if (category == null) return BadRequest($"Can not find category by id: {categoryId}");

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CategoryCreateRequest request)
        {
            Category category = new Category()
            {
                Name = request.Name,
            };

            var result = await _categoryService.Create(category);

            if (result > 0) return Ok("Add category success");

            return BadRequest("Fail to add category");
        }

        [HttpPatch("{categoryId}")]
        public async Task<ActionResult> Update(int categoryId, [FromBody] CategoryUpdateRequest request)
        {
            var category = await _categoryService.GetById(categoryId);

            if (category == null) return BadRequest($"Can not find category by id: {categoryId}");

            category.Name = request.Name;

            var result = await _categoryService.Update(category);

            if (result > 0) return Ok("Update category success");

            return BadRequest("Fail to update category");
        }

        [HttpDelete("{categoryId}")]
        public async Task<ActionResult> Remove(int categoryId)
        {
            var category = await _categoryService.GetById(categoryId);

            if (category == null) return BadRequest($"Can not find category by id: {categoryId}");

            var result = await _categoryService.Remove(category);

            if (result > 0) return Ok("Delete category success");

            return BadRequest("Fail to delete category");
        }
    }
}