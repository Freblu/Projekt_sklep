using Microsoft.EntityFrameworkCore;
using PartsCom.Application.Interfaces;
using PartsCom.Domain.Entities;
using PartsCom.Infrastructure.Database;

namespace PartsCom.Infrastructure.Repositories;

internal sealed class NewsRepository(PartsComDbContext dbContext) : INewsRepository
{
    public async Task<IEnumerable<NewsPost>> GetLatestAsync(int count, CancellationToken cancellationToken = default)
    {
        return await dbContext.NewsPosts
            .AsNoTracking()
            .OrderByDescending(n => n.PublishedAt)
            .Take(count)
            .ToListAsync(cancellationToken);
    }
}
