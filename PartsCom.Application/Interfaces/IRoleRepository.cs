using PartsCom.Domain.Entities;

namespace PartsCom.Application.Interfaces;

public interface IRoleRepository
{
    void Add(Role role);

    Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<Role>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default);

    void Update(Role role);

    void Delete(Role role);
}
