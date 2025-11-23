using System.Security.Claims;

namespace Inova.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(int userId, string email, string fullName, string role, int profileId);
    ClaimsPrincipal ValidateToken(string token);
}