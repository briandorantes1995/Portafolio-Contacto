using Microsoft.AspNetCore.Identity;
using Portafolio.Models;

namespace Portafolio.Services.Auth;

public class PasswordService : IPasswordService
{
    private readonly PasswordHasher<AppUser> _hasher = new();

    public string HashPassword(string password)
    {
        return _hasher.HashPassword(new AppUser(), password);
    }

    public bool VerifyPassword(string hash, string password)
    {
        var result = _hasher.VerifyHashedPassword(
            new AppUser(),
            hash,
            password
        );

        return result == PasswordVerificationResult.Success;
    }
}