namespace Inova.Application.DTOs.Chat;

public class SendMessageRequestDto
{
    public int SessionId { get; set; } // The ID of the chat session

    public string Message { get; set; } // The content of the message to be sent

}