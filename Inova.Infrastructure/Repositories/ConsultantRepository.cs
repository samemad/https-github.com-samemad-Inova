using Microsoft.EntityFrameworkCore;
using Inova.Domain.Entities;
using Inova.Domain.Repositories;
using Inova.Infrastructure.Data;

namespace Inova.Infrastructure.Repositories;

internal sealed class ConsultantRepository : IConsultantRepository
{
    private readonly ApplicationDbContext _context;

    public ConsultantRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Consultant> GetByIdAsync(int id)
    {
        return await _context.Consultants
            .Include(c => c.User)
            .Include(c => c.Specialization)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Consultant> GetByUserIdAsync(int userId)
    {
        return await _context.Consultants
            .Include(c => c.User)
            .Include(c => c.Specialization)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task<IEnumerable<Consultant>> GetAllAsync()
    {
        return await _context.Consultants
            .Include(c => c.User)
            .Include(c => c.Specialization)
            .ToListAsync();
    }

    public async Task<IEnumerable<Consultant>> GetAllApprovedAsync()
    {
        return await _context.Consultants
            .Include(c => c.User)
            .Include(c => c.Specialization)
            .Where(c => c.IsApproved == true)
            .ToListAsync();
    }

    public async Task<IEnumerable<Consultant>> GetPendingApprovalAsync()
    {
        return await _context.Consultants
            .Include(c => c.User)
            .Include(c => c.Specialization)
            .Where(c => c.ApprovalStatus == "Pending")
            .ToListAsync();
    }

    public async Task<IEnumerable<Consultant>> GetBySpecializationIdAsync(int specializationId)
    {
        return await _context.Consultants
            .Include(c => c.User)
            .Include(c => c.Specialization)
            .Where(c => c.SpecializationId == specializationId && c.IsApproved == true)
            .ToListAsync();
    }

    public async Task AddAsync(Consultant consultant)
    {
        consultant.CreatedAt = DateTime.UtcNow;
        consultant.ApprovalStatus = "Pending";
        consultant.IsApproved = false;

        await _context.Consultants.AddAsync(consultant);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Consultant consultant)
    {
        _context.Consultants.Update(consultant);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var consultant = await GetByIdAsync(id);
        if (consultant != null)
        {
            _context.Consultants.Remove(consultant);
            await _context.SaveChangesAsync();
        }
    }
}