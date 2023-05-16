using BusinessLayer.RequestDtos;
using FluentValidation;

namespace innogotchi_api.Validators
{
    public class UserUpdateProfileValidator : AbstractValidator<UserUpdateProfileDto>
    {
        public UserUpdateProfileValidator()
        {
            RuleFor(user => user.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name must be less than 50 characters.");

            RuleFor(user => user.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name must be less than 50 characters.");
        }
    }
}
