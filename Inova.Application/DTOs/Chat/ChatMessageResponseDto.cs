namespace Inova.Application.DTOs.Chat;

public class ChatMessageResponseDto
{
    public int Id { get; set; }
    public int SessionId { get; set; }

    // Sender information
    public int SenderId { get; set; }        
    public string SenderName { get; set; }

    // Message content
    public string MessageText { get; set; }
    public DateTime SentAt { get; set; }
    public bool IsRead { get; set; }
}