using Microsoft.EntityFrameworkCore;
using MsEmail.API.Context;
using MsEmail.API.Entities;
using MsEmail.API.Repository.Interface;
using NuGet.Protocol.Plugins;
using System.Linq.Expressions;

namespace MsEmail.API.Repository
{
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

        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User? GetById(long id)
        {
            return _context.Users.Find(id);
        }

        public User? GetByLogin(string email, string password)
        {
            return _context.Users.FirstOrDefault(x => x.Email == email && password == x.Password);
        }

        #region Dispose

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerable<User> Find(Expression<Func<User, bool>> expression)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
