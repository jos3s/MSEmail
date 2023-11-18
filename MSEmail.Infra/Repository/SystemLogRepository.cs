﻿using MsEmail.Domain.Entities.Common;
using MsEmail.Infra.Context;
using MSEmail.Domain.Interface;
using System.Linq.Expressions;

namespace MSEmail.Infra.Repository
{
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

        public IEnumerable<SystemLog> GetAll()
        {
            return _context.SystemLogs.ToList();
        }

        public SystemLog? GetById(long id)
        {
            return _context.SystemLogs.FirstOrDefault(x => x.Id == id);
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

        public IEnumerable<SystemLog> Find(Expression<Func<SystemLog, bool>> expression)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}