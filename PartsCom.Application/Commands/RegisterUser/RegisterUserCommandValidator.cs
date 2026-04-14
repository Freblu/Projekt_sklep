using FluentValidation;

namespace PartsCom.Application.Commands.RegisterUser;

internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(c => c.FirstName)
            .NotEmpty()
            .WithErrorCode("RU001")
            .WithMessage("First Name is required")
            .MaximumLength(100)
            .WithErrorCode("RU002")
            .WithMessage("First Name must not exceed 100 characters");

        RuleFor(c => c.LastName)
            .NotEmpty()
            .WithErrorCode("RU003")
            .WithMessage("Last Name is required")
            .MaximumLength(100)
            .WithErrorCode("RU004")
            .WithMessage("Last Name must not exceed 100 characters");

        RuleFor(c => c.Email)
            .NotEmpty()
            .WithErrorCode("RU004")
            .WithMessage("Email is required")
            .EmailAddress()
            .WithErrorCode("RU005")
            .WithMessage("Email must be a valid email address");

        RuleFor(c => c.Password)
            .NotEmpty()
            .WithErrorCode("RU006")
            .WithMessage("Password is required")
            .MinimumLength(8)
            .WithErrorCode("RU007")
            .WithMessage("Password must be at least 8 characters long")
            .Matches("[A-Z]")
            .WithErrorCode("RU008")
            .WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]")
            .WithErrorCode("RU009")
            .WithMessage("Password must contain at least one lowercase letter")
            .Matches("[0-9]")
            .WithErrorCode("RU010")
            .WithMessage("Password must contain at least one number")
            .Matches("[^a-zA-Z0-9]")
            .WithErrorCode("RU011")
            .WithMessage("Password must contain at least one special character");

        RuleFor(c => c.ConfirmPassword)
            .NotEmpty()
            .WithErrorCode("RU012")
            .WithMessage("Confirm Password is required")
            .Equal(c => c.Password)
            .WithErrorCode("RU013")
            .WithMessage("Confirm Password must match Password");
    }
}
