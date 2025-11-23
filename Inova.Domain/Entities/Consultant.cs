namespace Inova.Domain.Entities;

public class Consultant
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }

    public string FullName { get; set; }
    public string PhoneNumber { get; set; }

    public int SpecializationId { get; set; }
    public Specialization Specialization { get; set; }

    public decimal HourlyRate { get; set; }
    public string Bio { get; set; }
    public int YearsOfExperience { get; set; }

    // 📸 ADD THIS
    public string ProfileImageUrl { get; set; }      // Cloudinary URL for profile
    public string CoverImageUrl { get; set; }        // Optional: cover/banner image
    public string CertificateImageUrl { get; set; }  // Optional: professional certificate

    // Approval workflow
    public string ApprovalStatus { get; set; }
    public bool IsApproved { get; set; }
    public DateTime? ApprovedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<Session> Sessions { get; set; }
}