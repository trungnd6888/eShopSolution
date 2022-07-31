using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModel.System.Users
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