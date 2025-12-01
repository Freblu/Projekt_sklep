using PartsCom.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using PartsCom.Application.Interfaces;
using PartsCom.Infrastructure.Database;

namespace PartsCom.Infrastructure.Repositories;

internal sealed class UserRepository(PartsComDbContext dbContext) : IUserRepository
{
    public void Add(User user)
    {
        _ = dbContext.Users.Add(user);
    }

    public async Task<User?> GetByIdAsync(Guid? id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users.SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users.SingleOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users.SingleOrDefaultAsync(u => u.RefreshToken == refreshToken, cancellationToken);
    }

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Users.CountAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> GetRecentUsersAsync(int count = 10, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .Include(u => u.Orders)
            .OrderByDescending(u => u.CreatedAt)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public void Update(User user)
    {
        dbContext.Users.Update(user);
    }

    public Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return dbContext.Users.AnyAsync(u => u.Email == email, cancellationToken);
    }
}
