using Inova.Application.DTOs.Chat;
using Inova.Application.Interfaces;
using Inova.Application.Converters;
using Inova.Domain.Repositories;

namespace Inova.Application.Services;

internal sealed class ChatService : IChatService
{
    private readonly IChatMessageRepository _chatRepository;
    private readonly ISessionRepository _sessionRepository;

    public ChatService(
        IChatMessageRepository chatRepository,
        ISessionRepository sessionRepository)
    {
        _chatRepository = chatRepository;
        _sessionRepository = sessionRepository;
    }

  
    // SEND MESSAGE (with time-based access control)
   
    public async Task<ChatMessageResponseDto> SendMessageAsync(
        SendMessageRequestDto dto,
        int senderId)
    {
        // 1. Get session with details
        var session = await _sessionRepository.GetByIdWithDetailsAsync(dto.SessionId);

        if (session == null)
        {
            throw new InvalidOperationException($"Session {dto.SessionId} not found");
        }

        // 2. Verify user is part of this session
        var isCustomer = session.Customer.UserId == senderId;
        var isConsultant = session.Consultant.UserId == senderId;

        if (!isCustomer && !isConsultant)
        {
            throw new UnauthorizedAccessException(
                "You can only send messages in your own sessions");
        }

        // 3. Check session status
        if (session.Status != "Accepted")
        {
            throw new InvalidOperationException(
                $"Cannot send messages. Session status is '{session.Status}'. " +
                "Chat is only available for accepted sessions.");
        }

        // 4. Calculate session time window
        var sessionStart = session.ScheduledDate.Date + session.ScheduledTime;
        var sessionEnd = sessionStart.AddHours((double)session.DurationHours);
        var now = DateTime.UtcNow;

        // Optional: Add 15-minute grace period
        var gracePeriodMinutes = 15;
        var chatStartTime = sessionStart.AddMinutes(-gracePeriodMinutes);
        var chatEndTime = sessionEnd.AddMinutes(gracePeriodMinutes);

        // 5. Enforce time-based access control
        if (now < chatStartTime)
        {
            var minutesUntilStart = (chatStartTime - now).TotalMinutes;
            throw new InvalidOperationException(
                $"Chat not available yet. Session starts in {minutesUntilStart:F0} minutes.");
        }

        if (now > chatEndTime)
        {
            throw new InvalidOperationException(
                "Chat has ended. This session is now read-only.");
        }

        // 6. Validate message content
        if (string.IsNullOrWhiteSpace(dto.Message))
        {
            throw new InvalidOperationException("Message cannot be empty");
        }

        if (dto.Message.Length > 2000)
        {
            throw new InvalidOperationException("Message too long (max 2000 characters)");
        }

        // 7. Convert DTO to Entity
        var message = dto.ToEntity(senderId);

        // 8. Save message
        await _chatRepository.AddAsync(message);

        // 9. Reload with sender info for response
        message = await _chatRepository.GetByIdAsync(message.Id);

        // 10. Return DTO
        return message.ToResponseDto();
    }

   
    // GET SESSION MESSAGES (read-only access after session)
   
    public async Task<IEnumerable<ChatMessageResponseDto>> GetSessionMessagesAsync(
        int sessionId,
        int userId)
    {
        // 1. Get session
        var session = await _sessionRepository.GetByIdWithDetailsAsync(sessionId);

        if (session == null)
        {
            throw new InvalidOperationException($"Session {sessionId} not found");
        }

        // 2. Verify user is part of this session
        var isCustomer = session.Customer.UserId == userId;
        var isConsultant = session.Consultant.UserId == userId;

        if (!isCustomer && !isConsultant)
        {
            throw new UnauthorizedAccessException(
                "You can only view messages in your own sessions");
        }

        // 3. Get all messages for this session
        var messages = await _chatRepository.GetBySessionIdAsync(sessionId);

        // 4. Convert to DTOs
        return messages.Select(m => m.ToResponseDto());
    }

    
    // MARK MESSAGE AS READ
 
    public async Task<bool> MarkAsReadAsync(int messageId, int userId)
    {
        // 1. Get message
        var message = await _chatRepository.GetByIdAsync(messageId);

        if (message == null)
        {
            throw new InvalidOperationException($"Message {messageId} not found");
        }

        // 2. Get session to verify access
        var session = await _sessionRepository.GetByIdWithDetailsAsync(message.SessionId);

        // 3. Verify user is part of session
        var isCustomer = session.Customer.UserId == userId;
        var isConsultant = session.Consultant.UserId == userId;

        if (!isCustomer && !isConsultant)
        {
            throw new UnauthorizedAccessException("Access denied");
        }

        // 4. Mark as read
        await _chatRepository.MarkAsReadAsync(messageId);

        return true;
    }

    // GET UNREAD COUNT
  
    public async Task<int> GetUnreadCountAsync(int userId)
    {
        var unreadMessages = await _chatRepository.GetUnreadByUserIdAsync(userId);
        return unreadMessages.Count();
    }

  
    // REPORT SESSION (Bonus Feature)
    
    public async Task<bool> ReportSessionAsync(
        int sessionId,
        ReportSessionRequestDto dto,
        int reportedByUserId)
    {
        // 1. Get session
        var session = await _sessionRepository.GetByIdWithDetailsAsync(sessionId);

        if (session == null)
        {
            throw new InvalidOperationException($"Session {sessionId} not found");
        }

        // 2. Verify user is part of session
        var isCustomer = session.Customer.UserId == reportedByUserId;
        var isConsultant = session.Consultant.UserId == reportedByUserId;

        if (!isCustomer && !isConsultant)
        {
            throw new UnauthorizedAccessException(
                "You can only report sessions you're part of");
        }

        // 3. Check if already reported
        if (session.IsUnderReview)
        {
            throw new InvalidOperationException("This session is already under review");
        }

        // 4. Validate report reason
        if (string.IsNullOrWhiteSpace(dto.ReportReason))
        {
            throw new InvalidOperationException("Report reason is required");
        }

        // 5. Apply report using converter
        dto.ApplyReport(session, reportedByUserId);

        // 6. Save changes
        await _sessionRepository.UpdateAsync(session);

        return true;
    }
}