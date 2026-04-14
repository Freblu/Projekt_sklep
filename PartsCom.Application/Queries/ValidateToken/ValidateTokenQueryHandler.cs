using ErrorOr;
using MediatR;
using PartsCom.Application.Interfaces;

namespace PartsCom.Application.Queries.ValidateToken;

internal sealed class ValidateTokenQueryHandler(IJwtService jwtService) : IQueryHandler<ValidateTokenQuery>
{
    public Task<ErrorOr<Unit>> Handle(ValidateTokenQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(jwtService.VerifyToken(request.Token));
    }
}
