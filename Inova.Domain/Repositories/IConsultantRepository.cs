using Inova.Domain.Entities;

namespace Inova.Domain.Repositories;

public interface IConsultantRepository
{
    Task<Consultant> GetByIdAsync(int id);
    Task<Consultant> GetByUserIdAsync(int userId);
    Task<IEnumerable<Consultant>> GetAllAsync();
    Task<IEnumerable<Consultant>> GetAllApprovedAsync();
    Task<IEnumerable<Consultant>> GetPendingApprovalAsync();
    Task<IEnumerable<Consultant>> GetBySpecializationIdAsync(int specializationId);
    Task AddAsync(Consultant consultant);
    Task UpdateAsync(Consultant consultant);
    Task DeleteAsync(int id);
}