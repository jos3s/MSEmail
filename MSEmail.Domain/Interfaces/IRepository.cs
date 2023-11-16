using System.Linq.Expressions;

namespace MSEmail.Domain.Interface
{
    public interface IRepository<T> : IDisposable
    {
        public void Save();
        public IRepository<T> Insert(T entity);
        public IRepository<T> Update(T entity);
        public IRepository<T> Delete(long id);
        public IRepository<T> InsertRange(List<T> entitys);
        public IEnumerable<T> GetAll();
        public T? GetById(long id);
        public IEnumerable<T> Find(Expression<Func<T, bool>> expression);
    }
}
