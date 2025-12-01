using PartsCom.Application.Interfaces;

namespace PartsCom.Application.Commands.RegisterUser;

public sealed record RegisterUserCommand(string FirstName, string LastName, string Email, string Password, string ConfirmPassword) : ICommand;
