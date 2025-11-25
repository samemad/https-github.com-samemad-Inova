using Inova.Application.DTOs.Session;

namespace Inova.Application.Interfaces;

public interface ISessionService
{
    // Customer operations
    Task<SessionResponseDto> BookSessionAsync(BookSessionRequestDto dto, int customerId);
    Task<IEnumerable<SessionResponseDto>> GetMySessionsAsync(int customerId);
    Task<bool> CancelSessionAsync(int sessionId, int customerId);

    // Consultant operations
    Task<IEnumerable<SessionResponseDto>> GetMyConsultantSessionsAsync(int consultantId);
    Task<IEnumerable<SessionResponseDto>> GetPendingSessionsAsync(int consultantId);
    Task<bool> AcceptSessionAsync(int sessionId, int consultantId);
    Task<bool> DenySessionAsync(int sessionId, int consultantId);

    // Shared operations
    Task<SessionResponseDto> GetSessionByIdAsync(int id);
}