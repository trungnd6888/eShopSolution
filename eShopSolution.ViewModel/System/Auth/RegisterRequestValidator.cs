﻿using FluentValidation;

namespace eShopSolution.ViewModel.System.Auth
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().WithMessage("Fullname is required")
                .MaximumLength(200).WithMessage("First name can not over 200 characters");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is required")
                .MaximumLength(50).WithMessage("User name can not over 50 characters");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required")
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
                .WithMessage("Email format not match");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number is required");
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