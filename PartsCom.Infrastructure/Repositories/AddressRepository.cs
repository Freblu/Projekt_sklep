using Microsoft.EntityFrameworkCore;
using PartsCom.Application.Interfaces;
using PartsCom.Domain.Entities;
using PartsCom.Domain.Enums;
using PartsCom.Infrastructure.Database;

namespace PartsCom.Infrastructure.Repositories;

internal sealed class AddressRepository(PartsComDbContext dbContext) : IAddressRepository
{
    public void Add(Address address)
    {
        dbContext.Addresses.Add(address);
    }

    public async Task<Address?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Addresses
            .SingleOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Address>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Addresses
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.IsDefault)
            .ThenByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Address?> GetDefaultAddressAsync(Guid userId, AddressType type, CancellationToken cancellationToken = default)
    {
        return await dbContext.Addresses
            .Where(a => a.UserId == userId && a.IsDefault && (a.Type == type || a.Type == AddressType.Both))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public void Update(Address address)
    {
        dbContext.Addresses.Update(address);
    }

    public void Delete(Address address)
    {
        dbContext.Addresses.Remove(address);
    }

    public async Task UnsetDefaultAddressesAsync(Guid userId, AddressType type, CancellationToken cancellationToken = default)
    {
        List<Address> defaultAddresses = await dbContext.Addresses
            .Where(a => a.UserId == userId && a.IsDefault && (a.Type == type || a.Type == AddressType.Both))
            .ToListAsync(cancellationToken);

        foreach (Address address in defaultAddresses)
        {
            address.UnsetDefault();
        }
    }
}
