
using System.Linq.Expressions;

namespace TransactionalDomain.Interfaces
{
    public interface IRepositoryAsync<T> : IDisposable where T : class
    {
        Task<IEnumerable<T>> Get();
        Task<T> GetById(int id);
        Task<T> Insert(T entity);
        Task<T> Delete(int id);
        Task Update(T entity);
        Task<T> Find(Expression<Func<T,bool>> expression);
        Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> expression);

    }
}
