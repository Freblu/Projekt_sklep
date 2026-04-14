using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace PartsCom.Domain.Entities;

[Table("Users")]
[Index(nameof(Email), IsUnique = true)]
[Index(nameof(RefreshToken), IsUnique = true)]
public sealed class User
{
    private User() { }

    public static User Create(string firstName, string lastName, string email, string passwordHash,
        string? refreshToken = null, DateTime? refreshTokenExpiryTime = null)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PasswordHash = passwordHash,
            RefreshToken = refreshToken,
            RefreshTokenExpiryTime = refreshTokenExpiryTime,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void SetRefreshToken(string refreshToken, DateTime expiryTime)
    {
        RefreshToken = refreshToken;
        RefreshTokenExpiryTime = expiryTime;
    }

    public bool IsRefreshTokenExpired()
    {
        if (!RefreshTokenExpiryTime.HasValue)
        {
            return true;
        }

        return DateTime.UtcNow >= RefreshTokenExpiryTime.Value;
    }

    public bool IsRefreshTokenValid(string refreshToken)
    {
        return RefreshToken == refreshToken && !IsRefreshTokenExpired();
    }

    [Key]
    public Guid Id { get; private set; }

    [Required]
    [MaxLength(100)]
    public string FirstName { get; private set; }

    [Required]
    [MaxLength(100)]
    public string LastName { get; private set; }

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; private set; }

    [Required]
    [MaxLength(200)]
    public string PasswordHash { get; private set; }

    [MaxLength(44)]
    [Column(TypeName = "varchar(44)")]
    public string? RefreshToken { get; private set; }

    public DateTime? RefreshTokenExpiryTime { get; private set; }

    [MaxLength(20)]
    public string? PhoneNumber { get; private set; }

    [MaxLength(100)]
    public string? City { get; private set; }

    [MaxLength(500)]
    public string? AvatarUrl { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    // Relationships
    public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();
    public ICollection<Address> Addresses { get; private set; } = new List<Address>();
    public ICollection<Order> Orders { get; private set; } = new List<Order>();
    public ICollection<PaymentCard> PaymentCards { get; private set; } = new List<PaymentCard>();
    public ICollection<ProductReview> Reviews { get; private set; } = new List<ProductReview>();
    public ICollection<BrowsingHistory> BrowsingHistories { get; private set; } = new List<BrowsingHistory>();
    public Cart? Cart { get; private set; }

    public void UpdateName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        UpdatedAt = DateTime.UtcNow;
    }

    [SuppressMessage("Design", "CA1054:URI-like parameters should not be strings", Justification = "URL stored as string in database")]
    public void UpdateProfile(string? phoneNumber, string? city, string? avatarUrl)
    {
        PhoneNumber = phoneNumber;
        City = city;
        AvatarUrl = avatarUrl;
        UpdatedAt = DateTime.UtcNow;
    }

    [SuppressMessage("Design", "CA1054:URI-like parameters should not be strings", Justification = "URL stored as string in database")]
    public void UpdateFullProfile(string firstName, string lastName, string? phoneNumber, string? city, string? avatarUrl)
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        City = city;
        AvatarUrl = avatarUrl;
        UpdatedAt = DateTime.UtcNow;
    }
}
