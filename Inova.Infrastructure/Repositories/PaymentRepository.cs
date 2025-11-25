using Inova.Domain.Entities;
using Inova.Domain.Repositories;
using Inova.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

internal sealed class PaymentRepository : IPaymentRepository
{
    private readonly ApplicationDbContext _context;

    public PaymentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Payment payment)
    {
       payment.CreatedAt = DateTime.UtcNow;
       await _context.Payments.AddAsync(payment);
       await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var payment = await GetByIdAsync(id);
        if (payment != null)
        {
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Payment>> GetAllAsync()
    {
       return await _context.Payments.ToListAsync();
    }

    public async Task<Payment> GetByIdAsync(int id)
    {
       return await _context.Payments.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Payment> GetBySessionIdAsync(int sessionId)
    {
        return await _context.Payments.FirstOrDefaultAsync(p => p.SessionId == sessionId);
    }

    public async Task UpdateAsync(Payment payment)
    {
        _context.Payments.Update(payment);
        await _context.SaveChangesAsync();
    }
}