namespace Inova.Domain.Entities;

public class Session
{
    public int Id { get; set; }

    // Foreign Keys
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }

    public int ConsultantId { get; set; }
    public Consultant Consultant { get; set; }

    // Session Details
    public DateTime ScheduledDate { get; set; }
    public TimeSpan ScheduledTime { get; set; }
    public decimal DurationHours { get; set; }  // 1.0, 1.5, 2.0
    public decimal TotalAmount { get; set; }

    // Session Status
    public string Status { get; set; }  // "Pending", "Accepted", "Denied", "Completed", "Cancelled"

    public DateTime CreatedAt { get; set; }
    public DateTime? AcceptedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    // Navigation: Session has one Payment
    public Payment Payment { get; set; }

    // Navigation: Session has many ChatMessages
    public ICollection<ChatMessage> ChatMessages { get; set; }

    public bool IsUnderReview { get; set; }  // Default: false
    public string? ReportReason { get; set; }  // Why reported?
    public int? ReportedBy { get; set; }  // Who reported (UserId)
    public DateTime? ReportedAt { get; set; }  // When reported?
}