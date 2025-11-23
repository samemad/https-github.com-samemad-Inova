namespace Inova.Application.DTOs.Auth;

public sealed class LoginRequestDto
{
    public string Email { get; set; }
    public string Password { get; set; }

    public bool IsValid()
    {
        if (string.IsNullOrWhiteSpace(Email))
            return false;

        if (string.IsNullOrWhiteSpace(Password))
            return false;

        return true;
    }

    public string GetValidationErrors()
    {
        if (string.IsNullOrWhiteSpace(Email))
            return "Email is required";

        if (string.IsNullOrWhiteSpace(Password))
            return "Password is required";

        return string.Empty;
    }
}