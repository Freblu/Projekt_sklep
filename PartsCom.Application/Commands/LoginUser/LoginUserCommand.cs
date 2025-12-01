using PartsCom.Application.Interfaces;

namespace PartsCom.Application.Commands.LoginUser;

public sealed record LoginUserCommand(string Email, string Password) : ICommand<LoginUserCommandResponse>;
