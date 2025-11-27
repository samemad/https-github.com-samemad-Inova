namespace Inova.Application.DTOs.Chat;

public class ChatMessageResponseDto
{
    
    public int Id { get; set; } 

    public int SessionId { get; set; }

    public string SenderName { get; set; }
    public string MessageText { get; set; }
    public DateTime SentAt { get; set; }
    public bool IsRead { get; set; }

}