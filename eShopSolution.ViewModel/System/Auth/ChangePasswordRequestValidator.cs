using FluentValidation;

namespace eShopSolution.ViewModel.System.Auth
{
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage("Current Password is required");
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