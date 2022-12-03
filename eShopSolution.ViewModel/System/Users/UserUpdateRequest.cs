using Microsoft.AspNetCore.Http;

namespace eShopSolution.ViewModel.System.Users
{
    public class UserUpdateRequest
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public List<IFormFile>? AvatarImage { get; set; }
        public string? InputHidden { get; set; }
        public List<int>? Roles { get; set; }
    }
}
