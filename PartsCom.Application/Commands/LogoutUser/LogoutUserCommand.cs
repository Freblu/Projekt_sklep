using PartsCom.Application.Interfaces;

namespace PartsCom.Application.Commands.LogoutUser;

public sealed record LogoutUserCommand(string Token) : ICommand;
