namespace Inova.Domain.Entities;

public class Payment
{
    public int Id { get; set; }

    public int SessionId { get; set; }  // Foreign Key
    public Session Session { get; set; }  // Navigation Property

    public decimal Amount { get; set; }
    public string Status { get; set; }  // "Held", "Captured", "Released"

    public DateTime CreatedAt { get; set; }
    public DateTime? CapturedAt { get; set; }
    public DateTime? ReleasedAt { get; set; }
}