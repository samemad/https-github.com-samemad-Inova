using Inova.Application.DTOs.Category;
using Inova.Application.DTOs.Chat;
using Inova.Domain.Entities;

namespace Inova.Application.Converters;

internal static class ChatConverter
{

    // Direction 1: DTO → Entity (for CREATE)

    public static ChatMessage ToEntity(this SendMessageRequestDto dto)
    {
        return new ChatMessage
        {
           SessionId = dto.SessionId,
           MessageText = dto.Message

        };
    }

    public static ChatMessageResponseDto ToResponseDto(this ChatMessage message)

    {
        return new ChatMessageResponseDto
        {
            Id = message.Id,
            SessionId = message.SessionId,
            SenderName = message.Sender.FullName,
            MessageText = message.MessageText,
            SentAt = message.SentAt,
            IsRead = message.IsRead
        };

    }

    public static ReportSessionRequestDto ToEntity (this ReportSessionRequestDto dto)
    {
        return new ReportSessionRequestDto
        {
           ReportReason = dto.ReportReason
        };
    }

}