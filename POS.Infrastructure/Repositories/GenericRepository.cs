using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using POS.Domain.Interfaces;
using POS.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace POS.Infrastructure.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet=context.Set<T>();
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity!=null)
            {
                entity.IsDeleted = true;
                await UpdateAsync(entity);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.AnyAsync(x => x.Id == id);
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).AsNoTracking().ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
            => await _dbSet.FirstOrDefaultAsync(e => e.Id == id);

        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task UpdateAsync(T entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _dbSet.Update(entity);
        }
    }
}
