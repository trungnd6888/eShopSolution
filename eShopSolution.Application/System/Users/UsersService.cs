using eShopSolution.Application.Common;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace eShopSolution.Application.System.Users
{
    public class UsersService : IUsersService
    {
        private readonly EShopDbContext _context;
        private readonly IStorageService _storageService;

        public UsersService(EShopDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<int> Create(AppUser user)
        {
            _context.AppUsers.Add(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }

        public async Task<int> Update(AppUser user)
        {
            _context.Update(user);
            return await _context.SaveChangesAsync();
        }

        public async Task<AppUser> GetById(int? userId)
        {
            return await _context.AppUsers.FindAsync(userId);
        }

        public async Task<int> Remove(AppUser user)
        {
            _context.AppUsers.Remove(user);

            return await _context.SaveChangesAsync();
        }

        public IQueryable<AppUser> GetAll()
        {
            return _context.AppUsers;
        }

        public async Task<AppUser> GetByUserName(string userName)
        {
            var query = _context.AppUsers.Where(x => x.UserName.ToUpper().Trim() == userName.ToUpper().Trim());

            if (await query.CountAsync() == 0) return null;

            var product = await query.FirstAsync();

            return product;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

        public async Task<int> SaveChange()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
