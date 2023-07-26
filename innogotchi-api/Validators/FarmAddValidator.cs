using BusinessLayer.RequestDtos;
using FluentValidation;

namespace InnogotchiApi.Validators
{
    public class FarmAddValidator : AbstractValidator<FarmAddDto>
    {
        public FarmAddValidator()
        {
            RuleFor(farm => farm.Name)
                .NotEmpty().WithMessage("Farm name is required.")
                .MaximumLength(50).WithMessage("Farm name must be less than 50 characters.");
        }
    }
}
