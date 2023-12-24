using AldhamrimediaApi.Dto.UserDto;
using FluentValidation;


namespace AldhamrimediaApi.FluentValidation
{
    public class RegistraionUserValidator : AbstractValidator<RegistraionUserDto>
    {

        public RegistraionUserValidator()
        {

            RuleFor(x => x.Name).NotEmpty().NotEmpty().MaximumLength(50);
            RuleFor(x => x.Email).NotEmpty()
               .NotNull().EmailAddress();

            RuleFor(x => x.Password).NotEmpty().NotEmpty().MinimumLength(7);
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password);
            //RuleFor(x => x.PhoneNumper).MaximumLength(11).MinimumLength(11)
            //    .Must(IsValidPhoneNumper).WithMessage("{PropertyName} should be all numbers");



        }

        //private bool IsValidPhoneNumper(string PhoneNumper)
        //{
        //    return PhoneNumper.All(Char.IsNumber);
        //}
    }
}

