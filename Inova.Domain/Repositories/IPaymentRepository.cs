using Inova.Domain.Entities;

namespace Inova.Domain.Repositories;

public interface IPaymentRepository
{
    Task<Payment> GetByIdAsync(int id);
    Task<Payment> GetBySessionIdAsync(int sessionId);
    Task<IEnumerable<Payment>> GetAllAsync();
    Task AddAsync(Payment payment);
    Task UpdateAsync(Payment payment);
    Task DeleteAsync(int id);
}