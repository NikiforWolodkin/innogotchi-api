using BusinessLayer.RequestDtos;
using FluentValidation;

namespace InnogotchiApi.Validators
{
    public class InnogotchiAddValidator : AbstractValidator<InnogotchiAddDto>
    {
        public InnogotchiAddValidator()
        {
            RuleFor(inno => inno.Name)
                .NotEmpty().WithMessage("Innogotchi name is required.")
                .MaximumLength(50).WithMessage("Innogotchi name must be less than 50 characters.");
        }
    }
}
