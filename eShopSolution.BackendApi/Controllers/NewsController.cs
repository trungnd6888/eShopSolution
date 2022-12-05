using eShopSolution.Application.Catalog.Newses;
using eShopSolution.Application.Common.FileStorage;
using eShopSolution.Application.System.Users;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.Catalog.Newses;
using eShopSolution.ViewModel.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        private readonly IUserService _userService;
        private readonly IStorageService _storageService;

        public NewsController(INewsService newsService, IUserService userService, IStorageService storageService)
        {
            _newsService = newsService;
            _userService = userService;
            _storageService = storageService;
        }

        [HttpGet("/api/public/[controller]")]
        public async Task<ActionResult> GetPublic([FromQuery] NewsGetRequest request)
        {
            var query = from n in _newsService.GetAll()
                        join u in _userService.GetAll() on n.UserId equals u.Id
                        into table
                        from item in table.DefaultIfEmpty()
                        select new { n, item };

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.n.Title.Contains(request.Keyword.Trim())
                || x.n.Summary.Contains(request.Keyword.Trim())
                || x.n.Content.Contains(request.Keyword.Trim()));
            }

            var totalRecord = await query.CountAsync();

            var data = await query.Select(x => new NewsViewModel()
            {
                Id = x.n.Id,
                Title = x.n.Title,
                Summary = x.n.Summary,
                Content = x.n.Content,
                CreateDate = x.n.CreateDate,
                IsApproved = x.n.IsApproved,
                UserId = x.n.UserId,
                UserName = x.item.UserName,
                ImageUrl = !string.IsNullOrEmpty(x.n.ImageUrl)
                           ? _storageService.GetFileUrl(x.n.ImageUrl)
                           : string.Empty,
            }).ToListAsync();

            var news = new PageResult<NewsViewModel>()
            {
                Data = data,
                TotalRecord = totalRecord,
            };

            return Ok(news);
        }

        [HttpGet("/api/public/[controller]/{newsId}")]
        public async Task<ActionResult> GetByIdPublic(int newsId)
        {
            var news = await _newsService.GetById(newsId);

            if (news == null) return BadRequest($"Can not find news by id: {newsId}");

            var user = await _userService.GetById(news.UserId);

            var newsVM = new NewsViewModel()
            {
                Id = news.Id,
                Title = news.Title,
                Summary = news.Summary,
                Content = news.Content,
                CreateDate = news.CreateDate,
                ImageUrl = news.ImageUrl,
                IsApproved = news.IsApproved,
                UserId = news.UserId,
                UserName = user != null ? user.UserName : string.Empty,
                AvatarImage = user != null && !string.IsNullOrEmpty(user.AvatarImage)
                              ? _storageService.GetFileUrl(user.AvatarImage)
                              : string.Empty,
            };

            newsVM.ImageUrl = !string.IsNullOrEmpty(newsVM.ImageUrl)
                              ? _storageService.GetFileUrl(newsVM.ImageUrl)
                              : string.Empty;

            return Ok(newsVM);
        }

        [HttpGet]
        [Authorize(Policy = "NewsView")]
        public async Task<ActionResult> Get([FromQuery] NewsGetRequest request)
        {
            var query = from n in _newsService.GetAll()
                        join u in _userService.GetAll() on n.UserId equals u.Id
                        into table
                        from item in table.DefaultIfEmpty()
                        select new { n, item };

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.n.Title.Contains(request.Keyword.Trim())
                || x.n.Summary.Contains(request.Keyword.Trim())
                || x.n.Content.Contains(request.Keyword.Trim()));
            }

            var totalRecord = await query.CountAsync();

            var data = await query.Select(x => new NewsViewModel()
            {
                Id = x.n.Id,
                Title = x.n.Title,
                Summary = x.n.Summary,
                Content = x.n.Content,
                CreateDate = x.n.CreateDate,
                IsApproved = x.n.IsApproved,
                UserId = x.n.UserId,
                UserName = x.item.UserName,
                ImageUrl = !string.IsNullOrEmpty(x.n.ImageUrl)
                           ? _storageService.GetFileUrl(x.n.ImageUrl)
                           : string.Empty,
            }).ToListAsync();

            var news = new PageResult<NewsViewModel>()
            {
                Data = data,
                TotalRecord = totalRecord,
            };

            return Ok(news);
        }

        [HttpGet("{newsId}")]
        [Authorize(Policy = "NewsView")]
        public async Task<ActionResult> GetById(int newsId)
        {
            var news = await _newsService.GetById(newsId);

            if (news == null) return BadRequest($"Can not find news by id: {newsId}");

            news.ImageUrl = !string.IsNullOrEmpty(news.ImageUrl)
                            ? _storageService.GetFileUrl(news.ImageUrl)
                            : string.Empty;

            return Ok(news);
        }

        [HttpPost]
        [Authorize(Policy = "NewsView")]
        public async Task<ActionResult> Create([FromForm] NewsCreateRequest request)
        {
            News news = new News();
            news.Title = request.Title;
            news.Summary = request.Summary;
            news.Content = request.Content;
            news.IsApproved = request.IsApproved;
            news.UserId = request.UserId;
            news.ApprovedId = request.ApprovedId;
            news.CreateDate = DateTime.Now;

            if (request.ThumbnailImage != null)
            {
                news.ImageUrl = await this.SaveFile(request.ThumbnailImage[0]);
            }

            var result = await _newsService.Create(news);

            if (result > 0) return Ok("Add news success");

            return BadRequest("Fail to add news");
        }

        [HttpPatch("{newsId}")]
        [Authorize(Policy = "NewsView")]
        public async Task<ActionResult> Update(int newsId, [FromForm] NewsUpdateRequest request)
        {
            var news = await _newsService.GetById(newsId);

            if (news == null) return BadRequest($"Can not find news by id: {newsId}");

            news.Title = request.Title;
            news.Summary = request.Summary;
            news.Content = request.Content;
            news.IsApproved = request.IsApproved;

            //remove image 
            if (string.IsNullOrEmpty(request.InputHidden))
            {
                if (!string.IsNullOrEmpty(news.ImageUrl))
                {
                    await _storageService.DeleteFileAsync(news.ImageUrl);
                    news.ImageUrl = null;
                }
            }

            //add image
            if (request.ThumbnailImage != null)
            {
                //remove image after add
                if (!string.IsNullOrEmpty(news.ImageUrl))
                {
                    await _storageService.DeleteFileAsync(news.ImageUrl);
                }

                news.ImageUrl = await this.SaveFile(request.ThumbnailImage[0]);
            }

            var result = await _newsService.Update(news);

            if (result > 0) return Ok("Update news success");

            return BadRequest("Fail to update news");
        }

        [HttpDelete("{newsId}")]
        [Authorize(Policy = "NewsView")]
        public async Task<ActionResult> Remove(int newsId)
        {
            var news = await _newsService.GetById(newsId);

            if (news == null) return BadRequest($"Can not find news by id: {newsId}");

            var result = await _newsService.Remove(news);

            if (result > 0) return Ok("Remove news success");

            return BadRequest("Fail to remove news");
        }

        [HttpGet("new")]
        [Authorize(Policy = "NewsView")]
        public async Task<ActionResult> GetNew()
        {
            var news = await _newsService.GetAll().OrderByDescending(x => x.CreateDate).Take(5).ToListAsync();

            foreach (var item in news)
            {
                if (!string.IsNullOrEmpty(item.ImageUrl))
                {
                    item.ImageUrl = _storageService.GetFileUrl(item.ImageUrl);
                }
            }

            return Ok(news);
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
