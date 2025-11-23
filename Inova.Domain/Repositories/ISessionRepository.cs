using Inova.Domain.Entities;

namespace Inova.Domain.Repositories;

public interface ISessionRepository
{
    Task<Session> GetByIdAsync(int id);
    Task<Session> GetByIdWithDetailsAsync(int id);  // Include Customer, Consultant, Payment
    Task<IEnumerable<Session>> GetAllAsync();
    Task<IEnumerable<Session>> GetByCustomerIdAsync(int customerId);
    Task<IEnumerable<Session>> GetByConsultantIdAsync(int consultantId);
    Task<IEnumerable<Session>> GetPendingByConsultantIdAsync(int consultantId);
    Task AddAsync(Session session);
    Task UpdateAsync(Session session);
    Task DeleteAsync(int id);
}