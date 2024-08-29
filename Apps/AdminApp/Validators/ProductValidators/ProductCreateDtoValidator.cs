using FluentValidation;
using ShopAppAPI.Apps.AdminApp.Dtos.ProductDto;

namespace ShopAppAPI.Apps.AdminApp.Validators.ProductValidators
{
    public class ProductCreateDtoValidator:AbstractValidator<ProductCreateDto>
    {
        public ProductCreateDtoValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Name can't be empty")
                .MaximumLength(50).WithMessage("Maximum character length is 50");
            RuleFor(p => p.SalePrice).NotEmpty().WithMessage("Price can't be empty")
                .GreaterThan(p => p.CostPrice).WithMessage("Sell price can't be lower than cost price");    
            RuleFor(p => p.CostPrice).NotEmpty().WithMessage("Price can't be empty")
                .LessThan(p => p.SalePrice).WithMessage("Cost price can't be higher than sale price");
        }
    }
}
