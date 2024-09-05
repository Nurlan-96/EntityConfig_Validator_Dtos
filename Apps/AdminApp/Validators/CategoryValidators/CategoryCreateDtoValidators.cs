using FluentValidation;
using ShopAppAPI.Apps.AdminApp.Dtos.CategoryDto;

namespace ShopAppAPI.Apps.AdminApp.Validators.CategoryValidators
{
    public class CategoryCreateDtoValidators : AbstractValidator<CategoryCreateDto>
    {
        public CategoryCreateDtoValidators()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Name can't be empty").MaximumLength(50).WithMessage("Maximum Length can be 50");
            RuleFor(c => c).Custom((c, context) =>
            {
                if (!(c.Photo != null && c.Photo.ContentType.Contains("image/")))
                {
                    context.AddFailure("Photo", "only image..");
                }                
                if (!(c.Photo != null && c.Photo.Length/1024>500))
                {
                    context.AddFailure("Photo", "only image..");
                }
            });

    }
}
}
