using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniSozluk.Common.Models.RequestModels;

namespace UniSozluk.Api.Application.Features.Commands.User
{
    public class LoginUserCommandValidator:AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(i => i.EmailAdress).NotNull().EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible).WithMessage("{PropertyName} not a valid email adress");

            RuleFor(i => i.Password).NotNull().MinimumLength(6).WithMessage("{PropertyName} should at least be {MinLenght} characters");
        }
    }
}
