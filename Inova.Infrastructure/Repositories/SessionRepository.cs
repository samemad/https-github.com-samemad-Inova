using Inova.Domain.Entities;
using Inova.Domain.Repositories;
using Inova.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

internal sealed class SessionRepository : ISessionRepository
{
    private readonly ApplicationDbContext _context;

    public SessionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Session> GetByIdAsync(int id)
    {
       return await _context.Sessions.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Session> GetByIdWithDetailsAsync(int id)
    {
      return await _context.Sessions
            .Include(s => s.Customer)
            .Include(s => s.Consultant)
            .Include(s => s.Payment)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Session>> GetAllAsync()
    {
        return await _context.Sessions.ToListAsync();
    }

    public async Task<IEnumerable<Session>> GetByCustomerIdAsync(int customerId)
    {
        return await _context.Sessions
            .Where(s => s.CustomerId == customerId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Session>> GetByConsultantIdAsync(int consultantId)
    {
       return await _context.Sessions
            .Where(s => s.ConsultantId == consultantId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Session>> GetPendingByConsultantIdAsync(int consultantId)
    {
        return await _context.Sessions
            .Where(s => s.ConsultantId == consultantId && s.Status == "Pending")
            .ToListAsync();
    }

    public async Task AddAsync(Session session)
    {
       session.CreatedAt = DateTime.UtcNow;
       await _context.Sessions.AddAsync(session);
       await _context.SaveChangesAsync();
    }

    
    public async Task UpdateAsync(Session session)
    {
       _context.Sessions.Update(session);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
       var session = await GetByIdAsync(id);
        if ( session != null)
        {
            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();
        }
    }




}