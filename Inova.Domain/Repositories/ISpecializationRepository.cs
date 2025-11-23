using Inova.Domain.Entities;

namespace Inova.Domain.Repositories;

public interface ISpecializationRepository
{
    Task<Specialization> GetByIdAsync(int id);
    Task<IEnumerable<Specialization>> GetAllAsync();
    Task<IEnumerable<Specialization>> GetByCategoryIdAsync(int categoryId);
    Task AddAsync(Specialization specialization);
    Task UpdateAsync(Specialization specialization);
    Task DeleteAsync(int id);
}