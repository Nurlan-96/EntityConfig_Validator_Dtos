using FluentValidation;
using ShopAppAPI.Apps.AdminApp.Dtos.UserDto;

namespace ShopAppAPI.Apps.AdminApp.Validators.UserValidators
{
    public class LoginDtoValidator:AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(r => r.UserName).NotNull().NotEmpty().MaximumLength(30);
            RuleFor(r => r.Password).MaximumLength(20).MinimumLength(6);
        }
    }
}
