using PartsCom.Application.Interfaces;

namespace PartsCom.Application.Queries.GetUserDashboard;

public sealed record GetUserDashboardQuery(Guid UserId) : IQuery<GetUserDashboardQueryResponse>;
