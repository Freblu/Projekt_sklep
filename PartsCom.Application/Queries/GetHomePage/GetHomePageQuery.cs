using ErrorOr;
using PartsCom.Application.Interfaces;

namespace PartsCom.Application.Queries.GetHomePage;

public sealed record GetHomePageQuery : IQuery<GetHomePageQueryResponse>;
