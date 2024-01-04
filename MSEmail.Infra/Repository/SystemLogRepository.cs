using MsEmail.Domain.Entities.Common;
using MsEmail.Infra.Context;
using System.Linq.Expressions;
using MSEmail.Domain.Interfaces;

namespace MSEmail.Infra.Repository;

public class SystemLogRepository : IRepository<SystemLog>, IDisposable
{
    public AppDbContext _context { get; set; }

    public SystemLogRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Save()
    {
        _context.SaveChanges();
    }

    public IRepository<SystemLog> Insert(SystemLog entity)
    {
        entity.CreationDate = entity.UpdateDate = DateTime.Now;
        _context.SystemLogs.Add(entity);
        return this;
    }

    public IRepository<SystemLog> Update(SystemLog entity)
    {
        entity.UpdateDate = DateTime.Now;
        _context.SystemLogs.Update(entity);
        return this;
    }

    public IRepository<SystemLog> Delete(long id)
    {
        throw new NotImplementedException();
    }

    public IRepository<SystemLog> InsertRange(List<SystemLog> entitys)
    {
        throw new NotImplementedException();
    }

    public List<SystemLog> GetAll(bool withDeletionDate = false)
    {
        return _context.SystemLogs.ToList();
    }

    public SystemLog? GetById(long id)
    {
        return _context.SystemLogs.FirstOrDefault(x => x.Id == id);
    }

    public List<SystemLog> Find(Expression<Func<SystemLog, bool>> expression)
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
    #endregion
}