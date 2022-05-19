using FluentValidation;

namespace Application.Flows.Users.Queries
{
    public class FindUserByEmailValidator : AbstractValidator<FindUserByEmailCommand>
    {
        public FindUserByEmailValidator()
        {
            RuleFor(t => t.Email)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .EmailAddress().WithMessage("{PropertyName} must be a valid email address. Current value is: {PropertyValue}.");
        }
    }
}