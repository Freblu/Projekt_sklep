using PartsCom.Application.Interfaces;

namespace PartsCom.Application.Queries.GetUserAddresses;

public sealed record GetUserAddressesQuery(Guid UserId) : IQuery<GetUserAddressesQueryResponse>;
