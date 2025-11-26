using Microsoft.EntityFrameworkCore;
using Inova.Domain.Entities;
using Inova.Domain.Repositories;
using Inova.Infrastructure.Data;

namespace Inova.Infrastructure.Repositories
{
    internal sealed class ChatMessageRepository : IChatMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public ChatMessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ChatMessage> GetByIdAsync(int id)
        {
            return await _context.ChatMessages
                .Include(cm => cm.Sender)  // ← Include sender info!
                .FirstOrDefaultAsync(cm => cm.Id == id);
        }

        public async Task<IEnumerable<ChatMessage>> GetBySessionIdAsync(int sessionId)
        {
            return await _context.ChatMessages
                .Include(cm => cm.Sender)  // ← Include sender name
                .Where(cm => cm.SessionId == sessionId)
                .OrderBy(cm => cm.SentAt)  // ← Oldest first (chat order)
                .ToListAsync();
        }

        public async Task<IEnumerable<ChatMessage>> GetUnreadByUserIdAsync(int userId)
        {
            // Get all messages WHERE:
            // 1. I'm NOT the sender (someone sent TO me)
            // 2. Message is unread
            return await _context.ChatMessages
                .Include(cm => cm.Sender)
                .Include(cm => cm.Session)
                .Where(cm => cm.SenderId != userId && cm.IsRead == false)
                .ToListAsync();
        }

        public async Task AddAsync(ChatMessage message)
        {
            message.SentAt = DateTime.UtcNow;  // ← Auto timestamp
            message.IsRead = false;            // ← New messages are unread
            await _context.ChatMessages.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ChatMessage message)
        {
            _context.ChatMessages.Update(message);
            await _context.SaveChangesAsync();
        }

        public async Task MarkAsReadAsync(int messageId)
        {
            var message = await GetByIdAsync(messageId);
            if (message != null)
            {
                message.IsRead = true;
                await UpdateAsync(message);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var message = await GetByIdAsync(id);
            if (message != null)
            {
                _context.ChatMessages.Remove(message);
                await _context.SaveChangesAsync();
            }
        }
    }
}