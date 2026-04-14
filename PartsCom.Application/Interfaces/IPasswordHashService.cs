using ErrorOr;

namespace PartsCom.Application.Interfaces;

public interface IPasswordHashService
{
    ErrorOr<string> HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}
