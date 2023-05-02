using FluentValidation;
using innogotchi_api.Dtos;

namespace innogotchi_api.Validators
{
    public class UserValidator : AbstractValidator<UserRequestDto>
    {
        public UserValidator()
        {
            RuleFor(u => u.Email).NotEmpty();
            RuleFor(u => u.Password).NotEmpty();
            RuleFor(u => u.FirstName).NotEmpty();
            RuleFor(u => u.LastName).NotEmpty();
        }
    }
}
