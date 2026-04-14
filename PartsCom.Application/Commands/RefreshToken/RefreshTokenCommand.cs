using PartsCom.Application.Interfaces;

namespace PartsCom.Application.Commands.RefreshToken;

public sealed record RefreshTokenCommand(string RefreshToken) : ICommand<string>;
