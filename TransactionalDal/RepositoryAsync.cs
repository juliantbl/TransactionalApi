using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TransactionalDomain.Interfaces;

namespace TransactionalDal
{
    public partial class RepositoryAsync<T> : IRepositoryAsync<T> where T : class
    {
        private readonly TransactionsContext context;

        public RepositoryAsync(TransactionsContext _context)
        {
            context = _context;
        }

        protected DbSet<T> EntitySet 
        { 
            get { return context.Set<T>(); }
        }

        public async Task  Save()
        {
            await context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<IEnumerable<T>> Get()
        {
            return await EntitySet.ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await EntitySet.FindAsync(id);
        }

        public async Task<T> Insert(T entity)
        {
            EntitySet.Add(entity);
            await Save();
            return entity;
        }

        public async Task<T> Delete(int id)
        {
            T entity= await EntitySet.FindAsync(id);
            EntitySet.Remove(entity);
            await Save();
            return entity;
        }

        public async Task Update(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            await Save();
        }

        public async Task<T> Find(Expression<Func<T, bool>> expression)
        {
            return await EntitySet.AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public async Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> expression)
        {
            return await  EntitySet.AsNoTracking().Where(expression).ToListAsync();
        }
    }
}
