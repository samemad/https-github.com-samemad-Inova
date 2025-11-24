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
    internal sealed class CategoryRepository : ICategoryRepository

    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id );
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task AddAsync(Category category)
        {
            category.CreatedAt = DateTime.UtcNow;
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category)
        {
           _context.Categories.Update(category);
           await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
           var category = await GetByIdAsync(id);
              if (category != null)
              {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}
