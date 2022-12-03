using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.System.Auth;
using System.Collections;

namespace eShopSolution.Application.System.Auth
{
    public interface IAuthService
    {
        Task<AppUser> GetByUserName(string userName);

        Task<Hashtable> Authenticate(LoginRequest request);

        Task<bool> Register(RegisterRequest request);

        Task<Hashtable> ChangePassword(ChangePasswordRequest request);

        Task<bool> ForgotPassword(ForgotPasswordRequest request, string url);

        Task<Hashtable> ResetPassword(ResetPasswordRequest request, string token);

        Task SaveChange();
    }
}