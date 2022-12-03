namespace eShopSolution.ViewModel.System.Auth
{
    public class ChangePasswordRequest
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string CurrentPassword { get; set; }
        public int UserId { get; set; }

    }
}
