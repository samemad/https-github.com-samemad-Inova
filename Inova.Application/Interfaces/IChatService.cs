using Inova.Application.DTOs.Chat;

namespace Inova.Application.Interfaces;

public interface IChatService
{
    // Send a message (checks time-based access)
    Task<ChatMessageResponseDto> SendMessageAsync(
        SendMessageRequestDto dto,
        int senderId);

    // Get all messages for a session (read-only after session ends)
    Task<IEnumerable<ChatMessageResponseDto>> GetSessionMessagesAsync(
        int sessionId,
        int userId);

    // Mark message as read
    Task<bool> MarkAsReadAsync(int messageId, int userId);

    // Get unread message count for a user
    Task<int> GetUnreadCountAsync(int userId);

    // Report a session (bonus feature)
    Task<bool> ReportSessionAsync(
        int sessionId,
        ReportSessionRequestDto dto,
        int reportedByUserId);
}