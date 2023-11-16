using MsEmail.API.Entities;

namespace MsEmail.API.Repository.Interface
{
    public interface IGenericRepository<T> : IDisposable
    {
        public IEnumerable<T> GetAll();
        public T? GetById(int id);
        public IGenericRepository<T> Insert(T entity);
        public IGenericRepository<T> Delete(int id);
        public IGenericRepository<T> Update(T entity);
        public void Save();
    }
}
