using FluentValidation;

namespace eShopSolution.ViewModel.System.Auth
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            //RuleFor(x => x.Username).NotEmpty().WithMessage("User name is required");
            //RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
        }
    }
}