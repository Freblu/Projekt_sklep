using PartsCom.Domain.Entities;

namespace PartsCom.Application.Interfaces;

public interface INewsRepository
{
    Task<IEnumerable<NewsPost>> GetLatestAsync(int count, CancellationToken cancellationToken = default);
}
