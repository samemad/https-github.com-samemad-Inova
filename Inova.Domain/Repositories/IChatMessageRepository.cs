using Inova.Domain.Entities;

namespace Inova.Domain.Repositories;

public interface IChatMessageRepository
{
    Task<ChatMessage> GetByIdAsync(int id);
    Task<IEnumerable<ChatMessage>> GetBySessionIdAsync(int sessionId);
    Task<IEnumerable<ChatMessage>> GetUnreadByUserIdAsync(int userId);
    Task AddAsync(ChatMessage message);
    Task UpdateAsync(ChatMessage message);
    Task MarkAsReadAsync(int messageId);
    Task DeleteAsync(int id);
}