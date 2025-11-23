namespace Inova.Domain.Entities;

public class ChatMessage
{
    public int Id { get; set; }

    public int SessionId { get; set; }  // Foreign Key
    public Session Session { get; set; }  // Navigation Property

    public int SenderId { get; set; }  // Foreign Key to User
    public User Sender { get; set; }  // Navigation Property

    public string MessageText { get; set; }
    public DateTime SentAt { get; set; }
    public bool IsRead { get; set; }
}