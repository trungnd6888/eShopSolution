using FluentValidation;

namespace eShopSolution.ViewModel.System.Auth
{
    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required")
              .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
              .WithMessage("Email format not match");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
            RuleFor(x => x).Custom((request, context) =>
            {
                if (string.IsNullOrEmpty(request.ConfirmPassword))
                {
                    context.AddFailure("Confirm password is required");
                    return;
                }
                if (request.Password != request.ConfirmPassword)
                {
                    context.AddFailure("Confirm password is not match");
                }
            });
        }
    }
}