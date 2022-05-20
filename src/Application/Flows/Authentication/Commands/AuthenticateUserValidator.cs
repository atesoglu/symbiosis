using FluentValidation;

namespace Application.Flows.Authentication.Commands;

public class AuthenticateUserValidator : AbstractValidator<AuthenticateUserCommand>
{
    public AuthenticateUserValidator()
    {
        RuleFor(t => t.Email)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .EmailAddress().WithMessage("{PropertyName} must be a valid email address. Current value is: {PropertyValue}.");
            
        RuleFor(t => t.Password)
            .NotEmpty().WithMessage("{PropertyName} is required.");
    }
}