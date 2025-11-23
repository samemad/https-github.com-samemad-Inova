using Microsoft.EntityFrameworkCore;
using Inova.Domain.Entities;
using Inova.Domain.Repositories;
using Inova.Infrastructure.Data;

namespace Inova.Infrastructure.Repositories;

internal sealed class UserRepository : IUserRepository
{
	private readonly ApplicationDbContext _context;

	public UserRepository(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<User> GetByIdAsync(int id)
	{
		return await _context.Users
			.FirstOrDefaultAsync(u => u.Id == id);
	}

	public async Task<User> GetByEmailAsync(string email)
	{
		return await _context.Users
			.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
	}

	public async Task<IEnumerable<User>> GetAllAsync()
	{
		return await _context.Users.ToListAsync();
	}

	public async Task AddAsync(User user)
	{
		user.CreatedAt = DateTime.UtcNow;
		await _context.Users.AddAsync(user);
		await _context.SaveChangesAsync();
	}

	public async Task UpdateAsync(User user)
	{
		_context.Users.Update(user);
		await _context.SaveChangesAsync();
	}

	public async Task DeleteAsync(int id)
	{
		var user = await GetByIdAsync(id);
		if (user != null)
		{
			_context.Users.Remove(user);
			await _context.SaveChangesAsync();
		}
	}

	public async Task<bool> EmailExistsAsync(string email)
	{
		return await _context.Users
			.AnyAsync(u => u.Email.ToLower() == email.ToLower());
	}
}