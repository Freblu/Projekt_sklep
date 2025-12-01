namespace PartsCom.Domain.Entities;

/// <summary>
/// Join table for Many-to-Many relationship between User and Role
/// </summary>
public sealed class UserRole
{
    private UserRole() { }

    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    public Guid RoleId { get; private set; }
    public Role Role { get; private set; } = null!;

    public DateTime AssignedAt { get; private set; }

    public static UserRole Create(Guid userId, Guid roleId)
    {
        return new UserRole
        {
            UserId = userId,
            RoleId = roleId,
            AssignedAt = DateTime.UtcNow
        };
    }
}
