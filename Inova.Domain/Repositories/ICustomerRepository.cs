using Inova.Domain.Entities;

namespace Inova.Domain.Repositories;

public interface ICustomerRepository
{
    Task<Customer> GetByIdAsync(int id);
    Task<Customer> GetByUserIdAsync(int userId);
    Task<IEnumerable<Customer>> GetAllAsync();
    Task AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(int id);
}