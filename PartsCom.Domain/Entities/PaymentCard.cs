namespace PartsCom.Domain.Entities;

public sealed class PaymentCard
{
    private PaymentCard() { }

    public Guid Id { get; private set; }
    public string CardHolderName { get; private set; } = string.Empty;
    public string Last4Digits { get; private set; } = string.Empty;
    public string CardBrand { get; private set; } = string.Empty; // Visa, Mastercard, etc.
    public string ExpiryMonth { get; private set; } = string.Empty;
    public string ExpiryYear { get; private set; } = string.Empty;
    public bool IsDefault { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Relationships
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    public static PaymentCard Create(
        Guid userId,
        string cardHolderName,
        string last4Digits,
        string cardBrand,
        string expiryMonth,
        string expiryYear,
        bool isDefault = false)
    {
        return new PaymentCard
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CardHolderName = cardHolderName,
            Last4Digits = last4Digits,
            CardBrand = cardBrand,
            ExpiryMonth = expiryMonth,
            ExpiryYear = expiryYear,
            IsDefault = isDefault,
            CreatedAt = DateTime.UtcNow
        };
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

    public string GetMaskedCardNumber()
    {
        return $"**** **** **** {Last4Digits}";
    }
}
