namespace Inova.Application.DTOs.Profile;

public class UpdateConsultantProfileDto
{
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Bio { get; set; }
    public int YearsOfExperience { get; set; }
    public decimal HourlyRate { get; set; }
    public string ProfileImageUrl { get; set; } // Optional
    public string CoverImageUrl { get; set; } // Optional
    public string CertificateImageUrl { get; set; } // Optional
}