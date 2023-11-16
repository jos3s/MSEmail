using Microsoft.EntityFrameworkCore;
using MsEmail.API.Context;
using MsEmail.API.Entities;
using MsEmail.API.Repository.Interface;
using NuGet.Protocol.Plugins;

namespace MsEmail.API.Repository
{
    public class UserRepository : IGenericRepository<User>, IDisposable
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User? GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public IGenericRepository<User> Insert(User entity)
        {
            entity.CreationDate = entity.UpdateDate = DateTime.Now;
            _context.Users.Add(entity);
            return this;
        }

        public IGenericRepository<User> Update(User entity)
        {
            entity.UpdateDate = DateTime.Now;
            _context.Users.Update(entity);
            return this;
        }


        public User? GetByLogin(string email, string password)
        {
            return _context.Users.FirstOrDefault(x => x.Email == email && password == x.Password);
        }

        public IGenericRepository<User> Delete(int id)
        {
            User user = _context.Users.Find(id);
            user.DeletionDate = DateTime.Now;
            _context.Users.Remove(user);
            return this;
        }


        public void Save()
        {
            _context.SaveChanges();
        }

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

    }
}
