using eShopSolution.Application.Common.FileStorage;
using eShopSolution.Application.System.UserRoles;
using eShopSolution.Application.System.Users;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Exceptions;
using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "UserView")]
    public class UsersController : ControllerBase
    {
        private readonly IStorageService _storageService;
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;

        public UsersController(IUserService userService, IStorageService storageService, IUserRoleService userRoleService)
        {
            _userService = userService;
            _storageService = storageService;
            _userRoleService = userRoleService;
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] UserGetRequest request)
        {
            var query = _userService.GetAll();

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.UserName.Contains(request.Keyword.Trim())
                                  || x.Email.Contains(request.Keyword.Trim()));
            }

            //paging
            int totalRecord = await query.CountAsync();

            var data = await query.ToListAsync();

            //set avatarImage
            foreach (var item in data)
            {
                if (item.AvatarImage != null)
                {
                    item.AvatarImage = _storageService.GetFileUrl(item.AvatarImage);
                }
            }

            var users = new PageResult<AppUser>()
            {
                Data = data,
                TotalRecord = totalRecord
            };

            return Ok(users);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult> GetById(int userId)
        {
            var user = await _userService.GetById(userId);
            if (user == null) return BadRequest("Cannot find product");

            //set image
            if (user.AvatarImage != null)
            {
                user.AvatarImage = _storageService.GetFileUrl(user.AvatarImage);
            }

            //set userRoles
            var userRoles = await _userRoleService.GetByUserId(userId);

            List<int> roles = new List<int>();
            foreach (var item in userRoles) roles.Add(item.RoleId);

            var userVM = new UserViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FullName = user.FullName,
                AvatarImage = user.AvatarImage,
                UserRoles = roles,
            };

            return Ok(userVM);
        }

        [HttpPatch("{userId}")]
        [Authorize(Policy = "UserUpdate")]
        public async Task<ActionResult> Update(int userId, [FromForm] UserUpdateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //Check Code Product
            var userByUserName = await _userService.GetByUserName(request.UserName);

            if (userByUserName != null && userByUserName.Id != userId)
            {
                ModelState.AddModelError("userName", "UserName invalid");
                return BadRequest(ModelState);
            }

            //get old user
            AppUser? user = await _userService.GetById(userId);
            if (user == null) throw new EShopException($"Can not find a product by id: {userId}");

            /*update new user*/
            user.FullName = request.FullName;
            user.UserName = request.UserName;
            user.PhoneNumber = request.PhoneNumber;
            user.Email = request.Email;

            //Remove foreign key Old
            var userRolesRemove = await _userRoleService.GetByUserId(userId);

            foreach (var item in userRolesRemove)
            {
                _userRoleService.RemoveNotSave(item);
            }

            //Add foreign key New
            if (request.Roles != null && request.Roles.Count > 0)
            {
                foreach (var item in request.Roles)
                {
                    await _userRoleService.Add(new AppUserRole()
                    {
                        UserId = user.Id,
                        RoleId = item,
                    });
                }
            }

            //Remove image old
            if (string.IsNullOrEmpty(request.InputHidden))
            {
                if (user.AvatarImage != null)
                {
                    await _storageService.DeleteFileAsync(user.AvatarImage);
                }

                user.AvatarImage = null;
            }

            //Save image
            if (request.AvatarImage != null && request.AvatarImage.Count > 0)
            {
                //Add new
                user.AvatarImage = await this.SaveFile(request.AvatarImage[0]);
            }

            var result = await _userService.Update(user);
            if (result == 0) return BadRequest();

            return Ok();
        }

        [HttpDelete("{userId}")]
        [Authorize(Policy = "UserRemove")]
        public async Task<ActionResult> Remove(int userId)
        {
            var user = await _userService.GetById(userId);
            if (user == null) return BadRequest($"Can not find a user by id: {userId}");

            var result = await _userService.Remove(user);
            if (result == 0) return BadRequest("Fail to remove user");

            return Ok("Remove user success");
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
