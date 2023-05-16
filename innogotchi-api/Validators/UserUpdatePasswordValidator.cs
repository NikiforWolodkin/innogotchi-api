using BusinessLayer.RequestDtos;
using FluentValidation;

namespace innogotchi_api.Validators
{
    public class UserUpdatePasswordValidator : AbstractValidator<UserUpdatePasswordDto>
    {
        public UserUpdatePasswordValidator()
        {
            RuleFor(user => user.NewPassword)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");
        }
    }
}
