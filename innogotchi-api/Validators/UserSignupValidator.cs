using FluentValidation;
using DataLayer.RequestDtos;

namespace InnogotchiApi.Validators
{
    public class UserSignupValidator : AbstractValidator<UserSignupDto>
    {
        public UserSignupValidator()
        {
            {
                RuleFor(user => user.Email)
                    .NotEmpty().WithMessage("Email is required.")
                    .EmailAddress().WithMessage("Email must be a valid email address.");

                RuleFor(user => user.Password)
                    .NotEmpty().WithMessage("Password is required.")
                    .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");

                RuleFor(user => user.FirstName)
                    .NotEmpty().WithMessage("First name is required.")
                    .MaximumLength(50).WithMessage("First name must be less than 50 characters.");

                RuleFor(user => user.LastName)
                    .NotEmpty().WithMessage("Last name is required.")
                    .MaximumLength(50).WithMessage("Last name must be less than 50 characters.");
            }
        }
    }
}
