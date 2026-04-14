namespace PartsCom.Domain.Entities;

/// <summary>
/// Join table for Many-to-Many relationship between Role and Permission
/// </summary>
public sealed class RolePermission
{
    private RolePermission() { }

    public Guid RoleId { get; private set; }
    public Role Role { get; private set; } = null!;

    public Guid PermissionId { get; private set; }
    public Permission Permission { get; private set; } = null!;

    public DateTime AssignedAt { get; private set; }

    public static RolePermission Create(Guid roleId, Guid permissionId)
    {
        return new RolePermission
        {
            RoleId = roleId,
            PermissionId = permissionId,
            AssignedAt = DateTime.UtcNow
        };
    }
}
