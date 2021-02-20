using BookLibrary.Data;
using BookLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace BookLibrary.DAL
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T> GetById(Guid id)
        {
            return await _dbSet.SingleOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task Insert(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual async Task<List<T>> List()
        {
            /*
            var filtered = from e in _dbSet
                           where e.Author.Contains(author)
                           select e;
            */
            return await _dbSet.ToListAsync();
        }

        public virtual async Task Remove(Guid id)
        {
            T entityToDelete = await _dbSet.FindAsync(id);
            Remove(entityToDelete);
        }

        public virtual void Remove(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }

        public virtual async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
