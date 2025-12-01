using PartsCom.Domain.Entities;

namespace PartsCom.Application.Interfaces;

public interface IUserRepository
{
    void Add(User user);
    
    Task<User?> GetByIdAsync(Guid? id, CancellationToken cancellationToken = default);
    
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    
    Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);

    Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<User>> GetRecentUsersAsync(int count = 10, CancellationToken cancellationToken = default);

    void Update(User user);

    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
}
