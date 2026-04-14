using PartsCom.Domain.Enums;

namespace PartsCom.Domain.Entities;

public sealed class Address
{
    private Address() { }

    public Guid Id { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public string AddressLine1 { get; private set; } = string.Empty;
    public string? AddressLine2 { get; private set; }
    public string City { get; private set; } = string.Empty;
    public string PostalCode { get; private set; } = string.Empty;
    public string Country { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public AddressType Type { get; private set; }
    public bool IsDefault { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Relationships
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    public ICollection<Order> ShippingOrders { get; private set; } = new List<Order>();
    public ICollection<Order> BillingOrders { get; private set; } = new List<Order>();

    public static Address Create(
        Guid userId,
        string fullName,
        string addressLine1,
        string? addressLine2,
        string city,
        string postalCode,
        string country,
        string phoneNumber,
        AddressType type,
        bool isDefault = false)
    {
        return new Address
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            FullName = fullName,
            AddressLine1 = addressLine1,
            AddressLine2 = addressLine2,
            City = city,
            PostalCode = postalCode,
            Country = country,
            PhoneNumber = phoneNumber,
            Type = type,
            IsDefault = isDefault,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(
        string fullName,
        string addressLine1,
        string? addressLine2,
        string city,
        string postalCode,
        string country,
        string phoneNumber)
    {
        FullName = fullName;
        AddressLine1 = addressLine1;
        AddressLine2 = addressLine2;
        City = city;
        PostalCode = postalCode;
        Country = country;
        PhoneNumber = phoneNumber;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetAsDefault()
    {
        IsDefault = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UnsetDefault()
    {
        IsDefault = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
