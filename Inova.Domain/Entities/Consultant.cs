namespace Inova.Domain.Entities;

public class Consultant
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }

    public string FullName { get; set; }
    public string? PhoneNumber { get; set; }  // ← Make nullable

    public int SpecializationId { get; set; }
    public Specialization Specialization { get; set; }

    public decimal HourlyRate { get; set; }
    public string? Bio { get; set; }  // ← Make nullable
    public int YearsOfExperience { get; set; }

    // Images
    public string? ProfileImageUrl { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? CertificateImageUrl { get; set; }

    // Approval workflow
    public string ApprovalStatus { get; set; } //  "Pending", "Approved", "Rejected"
    public bool IsApproved { get; set; }
    public DateTime? ApprovedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<Session> Sessions { get; set; }
}