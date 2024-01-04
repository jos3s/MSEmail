using MsEmail.Domain.Entities;
using MsEmail.Infra.Context;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MSEmail.Domain.Interfaces;

namespace MSEmail.Infra.Repository;

public class UserRepository : IRepository<User>, IDisposable
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Save()
    {
        _context.SaveChanges();
    }

    public IRepository<User> Insert(User entity)
    {
        entity.CreationDate = entity.UpdateDate = DateTime.Now;
        _context.Users.Add(entity);
        return this;
    }

    public IRepository<User> Update(User entity)
    {
        entity.UpdateDate = DateTime.Now;
        _context.Users.Update(entity);
        return this;
    }

    public IRepository<User> Delete(long id)
    {
        User user = _context.Users.Find(id);
        user.DeletionDate = DateTime.Now;
        _context.Users.Update(user);
        return this;
    }

    public IRepository<User> InsertRange(List<User> entity)
    {
        throw new NotImplementedException();
    }

    public List<User> GetAll(bool withDeletionDate = false)
    {
        if(withDeletionDate) return _context.Users.ToList();
        return _context.Users.AsNoTracking().Where(x=> x.DeletionDate == null).ToList();
    }

    public User? GetById(long id)
    {
        return _context.Users.Find(id);
    }

    public User? GetByLogin(string email)
    {
        return _context.Users.FirstOrDefault(x => x.Email == email);
    }

    public List<User> Find(Expression<Func<User, bool>> expression)
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