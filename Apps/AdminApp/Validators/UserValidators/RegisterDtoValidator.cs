using FluentValidation;
using ShopAppAPI.Apps.AdminApp.Dtos.UserDto;

namespace ShopAppAPI.Apps.AdminApp.Validators.UserValidators
{
    public class RegisterDtoValidator:AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(r => r.Email).EmailAddress().NotEmpty();
            RuleFor(r => r.FullName).NotNull().NotEmpty().MaximumLength(30).WithMessage("Maximum Name Length can be 30");
            RuleFor(r=>r.UserName).NotNull().NotEmpty().MaximumLength(30).WithMessage("Maximum Name Length can be 30");
            RuleFor(r => r.Password).MaximumLength(20).MinimumLength(6);
            RuleFor(r => r).Custom((r, context) =>
            {
                if (r.Password != r.RePassword)
                {
                    context.AddFailure("Password", "Passwords do not match");
                }
            });
        }
    }
}
