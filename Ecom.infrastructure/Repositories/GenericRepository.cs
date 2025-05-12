using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Ecom.Core.Interfaces;
using Ecom.infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecom.infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        public GenericRepository(AppDbContext context)
        {
            _context = context;

        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.Set<T>().FindAsync(id);
            _context.Set<T>().Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().AsQueryable().AsNoTracking();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().AsQueryable().AsNoTracking();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            var entiy = await query.FirstOrDefaultAsync(x => EF.Property<int>(x, "Id") == id);
            return entiy;

        }

        public async Task UpdateAsync(T Entity)
        {
            _context.Entry(Entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
