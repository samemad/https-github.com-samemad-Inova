using Inova.Application.DTOs.Chat;
using Inova.Domain.Entities;

namespace Inova.Application.Converters;

internal static class ChatConverter
{
    
    // Direction 1: DTO → Entity (for CREATING messages)
    
    public static ChatMessage ToEntity(
        this SendMessageRequestDto dto,
        int senderId)  // ← Service will provide this from JWT token!
    {
        return new ChatMessage
        {
            SessionId = dto.SessionId,
            SenderId = senderId,      // ← FIXED!
            MessageText = dto.Message,
            SentAt = DateTime.UtcNow, // ← Auto-set
            IsRead = false            // ← New messages are unread
        };
    }


    // Direction 2: Entity → ResponseDTO (for READING messages)
 
    public static ChatMessageResponseDto ToResponseDto(
    this ChatMessage message,
    string customerName,
    string consultantName)
    {
        // Determine sender name based on who sent it
        string senderName = message.SenderId == message.Session.Customer.UserId
            ? customerName
            : consultantName;

        return new ChatMessageResponseDto
        {
            Id = message.Id,
            SessionId = message.SessionId,
            SenderId = message.SenderId,
            SenderName = senderName,
            MessageText = message.MessageText,
            SentAt = message.SentAt,
            IsRead = message.IsRead
        };
    }

   
    // REPORTING: DTO → Update Session Entity
   
    public static void ApplyReport(
        this ReportSessionRequestDto dto,
        Session session,
        int reportedByUserId)  // ← Who's reporting
    {
        session.IsUnderReview = true;
        session.ReportReason = dto.ReportReason;
        session.ReportedBy = reportedByUserId;
        session.ReportedAt = DateTime.UtcNow;
    }
}