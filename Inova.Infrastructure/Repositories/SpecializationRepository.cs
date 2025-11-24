using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Inova.Domain.Entities;
using Inova.Domain.Repositories;
using Inova.Infrastructure.Data;

namespace Inova.Infrastructure.Repositories
{
    internal sealed class SpecializationRepository : ISpecializationRepository
    {
        private readonly ApplicationDbContext _context;

        public SpecializationRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Specialization> GetByIdAsync(int id)
        {
            return await _context.Specializations
                .Include(s => s.Category)  // ← Include for details page
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Specialization>> GetAllAsync()
        {
            return await _context.Specializations
                .Include(s => s.Category)  // ← Include for listing page
                .ToListAsync();
        }

        public async Task<IEnumerable<Specialization>> GetByCategoryIdAsync(int categoryId)
        {
            return await _context.Specializations
                .Where(s => s.CategoryId == categoryId)
                // No Include needed - we already filtered by category!
                .ToListAsync();
        }
        public async Task AddAsync(Specialization specialization)
        {
            specialization.CreatedAt = DateTime.UtcNow;
            await _context.Specializations.AddAsync(specialization);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Specialization specialization)
        {
             _context.Specializations.Update(specialization);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
           var specialization = await GetByIdAsync(id);

            if (specialization != null)
            {
                _context.Specializations.Remove(specialization);
                await _context.SaveChangesAsync();
            }
        }
    }
}
