namespace Inova.Application.DTOs.Profile;

public class ConsultantProfileResponseDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string ProfileImageUrl { get; set; }

    // Consultant-specific fields
    public string SpecializationName { get; set; }
    public string Bio { get; set; }
    public int YearsOfExperience { get; set; }
    public decimal HourlyRate { get; set; }
    public string ApprovalStatus { get; set; }
    public bool IsApproved { get; set; }
    public int TotalSessions { get; set; }
    public double Rating { get; set; } = 0.0; // Future feature
}