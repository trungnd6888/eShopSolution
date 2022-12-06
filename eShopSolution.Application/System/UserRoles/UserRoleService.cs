﻿using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShopSolution.Application.System.UserRoles
{
    public class UserRoleService : IUserRoleService
    {
        private readonly EShopDbContext _context;
        public UserRoleService(EShopDbContext context)
        {
            _context = context;
        }

        //public async Task<int> Add(IdentityUserRole<int> userRole)
        //{
        //    _context.UserRoles.Add(userRole);
        //    return await _context.SaveChangesAsync();
        //}

        //public async Task<List<IdentityUserRole<int>>> GetByUserId(int userId)
        //{
        //    return await _context.UserRoles.Where(x => x.UserId == userId).ToListAsync();
        //}

        //public void RemoveNotSave(IdentityUserRole<int> userRole)
        //{
        //    _context.UserRoles.Remove(userRole);
        //}



        public async Task<int> Add(AppUserRole userRole)
        {
            _context.UserRoles.Add(userRole);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<AppUserRole>> GetByUserId(int userId)
        {
            return await _context.AppUserRoles.Where(x => x.UserId == userId).ToListAsync();
        }

        public void RemoveNotSave(AppUserRole userRole)
        {
            _context.AppUserRoles.Remove(userRole);
        }
    }
}