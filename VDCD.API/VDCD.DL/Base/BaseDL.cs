using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDCD.DL.ConnectDB;
using VDCD.DL.Interface;

namespace VDCD.DL.Base
{
    public class BaseDL<T> : IBaseDL<T> where T : class
    {
        protected readonly DBContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseDL(DBContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<int> InsertAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return await _context.SaveChangesAsync();
        }

        public virtual async Task<int> UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        public virtual async Task<int> DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return 0;

            _dbSet.Remove(entity);
            return await _context.SaveChangesAsync();
        }

        public virtual async Task<bool> AnyAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }
    }
}
