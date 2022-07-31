using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace eShopSolution.Application.System.User
{
    public interface IUserService
    {
        Task<AppUser> GetByUserName(string userName);

        Task<Hashtable> Authenticate(LoginRequest request);

        Task<bool> Register(RegisterRequest request);
    }
}