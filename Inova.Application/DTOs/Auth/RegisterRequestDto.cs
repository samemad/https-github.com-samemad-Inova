namespace Inova.Application.DTOs.Auth;

public sealed class RegisterRequestDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Role { get; set; }  // "Customer" or "Consultant"

    // Only for Consultant registration
    public int? SpecializationId { get; set; }
    public string? Bio { get; set; }
    public int? YearsOfExperience { get; set; }

    public bool IsValid()
    {
        if (string.IsNullOrWhiteSpace(Email) || !Email.Contains("@"))
            return false;

        if (string.IsNullOrWhiteSpace(Password) || Password.Length < 6)
            return false;

        if (string.IsNullOrWhiteSpace(FullName))
            return false;

        if (string.IsNullOrWhiteSpace(Role))
            return false;

        // If registering as Consultant, validate consultant fields
        if (Role.Equals("Consultant", StringComparison.OrdinalIgnoreCase))
        {
            if (!SpecializationId.HasValue || SpecializationId.Value <= 0)
                return false;

            if (string.IsNullOrWhiteSpace(Bio))
                return false;

            if (!YearsOfExperience.HasValue || YearsOfExperience.Value < 0)
                return false;
        }

        return true;
    }

    public string GetValidationErrors()
    {
        if (string.IsNullOrWhiteSpace(Email))
            return "Email is required";

        if (!Email.Contains("@"))
            return "Invalid email format";

        if (string.IsNullOrWhiteSpace(Password))
            return "Password is required";

        if (Password.Length < 6)
            return "Password must be at least 6 characters";

        if (string.IsNullOrWhiteSpace(FullName))
            return "Full name is required";

        if (string.IsNullOrWhiteSpace(Role))
            return "Role is required (Customer or Consultant)";

        if (Role.Equals("Consultant", StringComparison.OrdinalIgnoreCase))
        {
            if (!SpecializationId.HasValue || SpecializationId.Value <= 0)
                return "Specialization is required for consultants";

            if (string.IsNullOrWhiteSpace(Bio))
                return "Bio is required for consultants";

            if (!YearsOfExperience.HasValue || YearsOfExperience.Value < 0)
                return "Years of experience is required for consultants";
        }

        return string.Empty;
    }
}