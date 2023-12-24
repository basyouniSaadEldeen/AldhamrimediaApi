using AldhamrimediaApi.Dtos.UserDto;
using FluentValidation;


namespace AldhamrimediaApi.FluentValidation
{
    public class LoginValidator : AbstractValidator<LoginUserDto>
    {
        public LoginValidator()
        {



            RuleFor(x => x.Password)
               .NotNull().NotEmpty().WithMessage("the email or password is incorrect");

            RuleFor(x => x.Email).NotEmpty().EmailAddress().NotNull().WithMessage("the email or password is incorrect");

        }
    }
}
