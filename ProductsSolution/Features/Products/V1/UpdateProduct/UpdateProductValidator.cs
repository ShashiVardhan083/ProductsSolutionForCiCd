using FastEndpoints;
using FluentValidation;

namespace ProductsSolution.Features.Products.V1.UpdateProduct
{
    public class UpdateProductValidator : Validator<UpdateProductRequest>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Product name cannot be empty.")
                .MinimumLength(3)
                .WithMessage("Product name must be at least 3 characters long.")
                .When(x => x.Name != null);

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greater than 0.")
                .When(x => x?.Price != null);

            RuleFor(x => x.IsAvailable)
                .NotNull()
                .WithMessage("Availability status cannot be null.")
                .When(x => x?.IsAvailable != null);
        }
    }
}