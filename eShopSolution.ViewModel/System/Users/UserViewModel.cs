namespace eShopSolution.ViewModel.System.Users
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string? AvatarImage { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public List<int> Products { get; set; }
        public List<int> News { get; set; }
        public List<int> UserRoles { get; set; }
    }
}
