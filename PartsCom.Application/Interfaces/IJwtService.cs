using ErrorOr;
using MediatR;
using PartsCom.Domain.Entities;

namespace PartsCom.Application.Interfaces;

public interface IJwtService
{
    ErrorOr<string> GenerateToken(User user);
    string GenerateRefreshToken();
    ErrorOr<Unit> VerifyToken(string token);
    Guid? GetUserIdFromToken(string token);
}
