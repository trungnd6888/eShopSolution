namespace eShopSolution.ViewModel.System.RoleClaims
{
    public class RoleClaimCreateRequest
    {
        public int RoleId { get; set; }
        public string? ClaimType { get; set; }
        public string? ClaimValue { get; set; }
    }
}
