using PartsCom.Domain.Entities;
using PartsCom.Domain.Enums;

namespace PartsCom.Application.Interfaces;

public interface IAddressRepository
{
    void Add(Address address);

    Task<Address?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IEnumerable<Address>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<Address?> GetDefaultAddressAsync(Guid userId, AddressType type, CancellationToken cancellationToken = default);

    void Update(Address address);

    void Delete(Address address);

    Task UnsetDefaultAddressesAsync(Guid userId, AddressType type, CancellationToken cancellationToken = default);
}
