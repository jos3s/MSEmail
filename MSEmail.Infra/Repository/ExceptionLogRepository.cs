using MsEmail.Domain.Entities.Common;
using MsEmail.Infra.Context;
using MSEmail.Domain.Interface;
using System.Linq.Expressions;

namespace MSEmail.Infra.Repository
{
    public class ExceptionLogRepository : IRepository<ExceptionLog>, IDisposable
    {
        public AppDbContext _context { get; set; }

        public ExceptionLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public IRepository<ExceptionLog> Insert(ExceptionLog entity)
        {
            entity.CreationDate = entity.UpdateDate = DateTime.Now;
            _context.ExceptionLogs.Add(entity);
            return this;
        }

        public IRepository<ExceptionLog> Update(ExceptionLog entity)
        {
            throw new NotImplementedException();
        }

        public IRepository<ExceptionLog> Delete(long id)
        {
            throw new NotImplementedException();
        }

        public IRepository<ExceptionLog> InsertRange(List<ExceptionLog> entitys)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ExceptionLog> GetAll()
        {
            throw new NotImplementedException();
        }

        public ExceptionLog? GetById(long id)
        {
            throw new NotImplementedException();
        }

        #region Dispose
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerable<ExceptionLog> Find(Expression<Func<ExceptionLog, bool>> expression)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
