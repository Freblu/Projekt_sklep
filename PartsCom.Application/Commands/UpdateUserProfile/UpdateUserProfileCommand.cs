using System.Diagnostics.CodeAnalysis;
using PartsCom.Application.Interfaces;

namespace PartsCom.Application.Commands.UpdateUserProfile;

[SuppressMessage("Design", "CA1054:URI-like parameters should not be strings", Justification = "URL stored as string in database")]
public sealed record UpdateUserProfileCommand(
    Guid UserId,
    string FirstName,
    string LastName,
    string? PhoneNumber,
    string? City,
    string? AvatarUrl
) : ICommand;
