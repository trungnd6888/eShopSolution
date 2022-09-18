using eShopSolution.Application.Common;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace eShopSolution.Application.System.Roles
{
    public class RoleService : IRoleService
    {
        private readonly EShopDbContext _context;
        private readonly IStorageService _storageService;

        public RoleService(EShopDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<int> Create(AppRole role)
        {
            _context.AppRoles.Add(role);
            await _context.SaveChangesAsync();
            return role.Id;
        }

        public async Task<int> Update(AppRole role)
        {
            _context.Update(role);
            return await _context.SaveChangesAsync();
        }

        public async Task<AppRole> GetById(int roleId)
        {
            return await _context.AppRoles.FindAsync(roleId);
        }

        public async Task<int> Remove(AppRole role)
        {
            _context.AppRoles.Remove(role);

            return await _context.SaveChangesAsync();
        }

        public IQueryable<AppRole> GetAll()
        {
            return _context.AppRoles;
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
