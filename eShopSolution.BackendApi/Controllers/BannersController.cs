using eShopSolution.Application.Common;
using eShopSolution.Application.System.Banners;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.System.Banners;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class BannersController : ControllerBase
    {
        private readonly IBannerService _bannerService;
        private readonly IStorageService _storageService;

        public BannersController(IBannerService bannerService, IStorageService storageService)
        {
            _bannerService = bannerService;
            _storageService = storageService;
        }

        [HttpGet("/api/public/[controller]")]
        public async Task<ActionResult> GetPublic([FromQuery] BannerGetRequest request)
        {
            var query = _bannerService.GetAll();

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.Title.Contains(request.Keyword.Trim())
                || x.Summary.Contains(request.Keyword.Trim()));
            }

            var totalRecord = await query.CountAsync();

            var data = await query.Select(x => new Banner()
            {
                Id = x.Id,
                Title = x.Title,
                Summary = x.Summary,
                IsApproved = x.IsApproved,
                ImageUrl = !string.IsNullOrEmpty(x.ImageUrl) ? _storageService.GetFileUrl(x.ImageUrl) : string.Empty,
                Order = x.Order,
            }).Where(x => x.IsApproved == true).ToListAsync();

            var banners = new PageResult<Banner>()
            {
                Data = data,
                TotalRecord = totalRecord,
            };

            return Ok(banners);
        }

        [HttpGet]
        [Authorize(Policy = "BannerView")]
        public async Task<ActionResult> Get([FromQuery] BannerGetRequest request)
        {
            var query = _bannerService.GetAll();

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.Title.Contains(request.Keyword.Trim())
                || x.Summary.Contains(request.Keyword.Trim()));
            }

            var totalRecord = await query.CountAsync();

            var data = await query.Select(x => new Banner()
            {
                Id = x.Id,
                Title = x.Title,
                Summary = x.Summary,
                IsApproved = x.IsApproved,
                ImageUrl = !string.IsNullOrEmpty(x.ImageUrl) ? _storageService.GetFileUrl(x.ImageUrl) : string.Empty,
                Order = x.Order,
            }).ToListAsync();



            var banners = new PageResult<Banner>()
            {
                Data = data,
                TotalRecord = totalRecord,
            };

            return Ok(banners);
        }

        [HttpGet("{bannerId}")]
        [Authorize(Policy = "BannerView")]
        public async Task<ActionResult> GetById(int bannerId)
        {
            var banner = await _bannerService.GetById(bannerId);

            if (banner == null) return BadRequest($"Can not find a banner id: {bannerId}");

            banner.ImageUrl = !string.IsNullOrEmpty(banner.ImageUrl) ? _storageService.GetFileUrl(banner.ImageUrl) : string.Empty;

            return Ok(banner);
        }

        [HttpPost]
        [Authorize(Policy = "BannerView")]
        public async Task<ActionResult> Create([FromForm] BannerCreateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var banner = new Banner()
            {
                Title = request.Title,
                Summary = request.Summary,
                IsApproved = request.IsApproved,
                Order = request.Order,
            };

            if (request.ThumbnailImage != null)
            {
                banner.ImageUrl = await this.SaveFile(request.ThumbnailImage[0]);
            }

            var result = await _bannerService.Create(banner);

            if (result > 0) return Ok("Success to create banner");

            return BadRequest("Fail to create banner");
        }

        [HttpPatch("{bannerId}")]
        [Authorize(Policy = "BannerView")]
        public async Task<ActionResult> Update(int bannerId, [FromForm] BannerUpdateRequest request)
        {
            var banner = await _bannerService.GetById(bannerId);

            if (banner == null) return BadRequest($"Can not find banner by id: {bannerId}");

            banner.Title = request.Title;
            banner.Summary = request.Summary;
            banner.IsApproved = request.IsApproved;
            banner.Order = request.Order;

            //remove image 
            if (string.IsNullOrEmpty(request.inputHidden))
            {
                if (!string.IsNullOrEmpty(banner.ImageUrl))
                {
                    await _storageService.DeleteFileAsync(banner.ImageUrl);
                    banner.ImageUrl = null;
                }
            }

            //add image
            if (request.ThumbnailImage != null)
            {
                //remove image after add
                if (!string.IsNullOrEmpty(banner.ImageUrl))
                {
                    await _storageService.DeleteFileAsync(banner.ImageUrl);
                }

                banner.ImageUrl = await this.SaveFile(request.ThumbnailImage[0]);
            }

            var result = await _bannerService.Update(banner);

            if (result > 0) return Ok("Success to update banner");

            return BadRequest("Fail to update request");
        }

        [HttpDelete("{bannerId}")]
        [Authorize(Policy = "BannerView")]
        public async Task<ActionResult> Remove(int bannerId)
        {
            var banner = await _bannerService.GetById(bannerId);

            if (banner == null) return BadRequest($"Can not find banner by id: {bannerId}");

            var result = await _bannerService.Remove(banner);

            if (result > 0) return Ok("Success to remove banner");

            return BadRequest("Fail to remove banner");
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
