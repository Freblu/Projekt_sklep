using FluentValidation;

namespace PartsCom.Application.Commands.LoginUser;

internal sealed class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty()
            .WithErrorCode("LU001")
            .WithMessage("Email is required")
            .EmailAddress()
            .WithErrorCode("LU002")
            .WithMessage("Email is not valid");

        RuleFor(u => u.Password)
            .NotEmpty()
            .WithErrorCode("LU003")
            .WithMessage("Password is required");
    }
}
