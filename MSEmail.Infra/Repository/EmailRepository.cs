﻿using MsEmail.Domain.Entities;
using MsEmail.Infra.Context;
using MSEmail.Domain.Enums;
using MSEmail.Domain.Interface;
using System.Linq.Expressions;

namespace MSEmail.Infra.Repository
{
    public class EmailRepository : IRepository<Email>, IDisposable
    {
        public AppDbContext _context { get; set; }

        public EmailRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public IRepository<Email> Insert(Email entity)
        {
            entity.CreationDate = entity.UpdateDate = DateTime.Now;
            _context.Emails.Add(entity);
            return this;
        }

        public IRepository<Email> Update(Email entity)
        {
            entity.UpdateDate = DateTime.Now;
            _context.Emails.Update(entity);
            return this;
        }

        public IRepository<Email> Delete(long id)
        {
            Email email = _context.Emails.Find(id);
            email.DeletionDate = DateTime.Now;
            _context.Emails.Update(email);
            return this;
        }

        public IRepository<Email> InsertRange(List<Email> entitys)
        {
            entitys.ForEach(x =>
            {
                x.CreationDate = x.UpdateDate = DateTime.Now;
            });
            _context.Emails.AddRange(entitys);
            return this;
        }

        public List<Email> GetAll(bool withDeletionDate = false)
        {
            if(withDeletionDate)  return _context.Emails.ToList();
            return _context.Emails.Where(x => x.DeletionDate == null).ToList();
        }

        public Email? GetById(long id)
        {
            return _context.Emails.FirstOrDefault(x => x.Id == id);
        }

        public List<Email> GetEmailsByUserId(long userId, bool withDeletionDate = false)
        {
            if (withDeletionDate) return _context.Emails.Where(x => x.CreationUserId == userId).ToList();
            return _context.Emails.Where(x => x.DeletionDate == null && x.CreationUserId == userId).ToList();
        }

        public List<Email> Find(Expression<Func<Email, bool>> expression)
        {
            return _context.Emails.Where(expression).ToList();
        }

        public List<Email> GetEmailsByStatus(EmailStatus status)
        {
            return _context.Emails.Where(x => x.Status.Equals(status)).ToList();
        }

        public IRepository<Email> UpdateStatus(Email email)
        {
            email.UpdateDate = DateTime.Now;
            var dbEntry = _context.Entry(email);
            dbEntry.Property(x => x.Status).IsModified = true;
            dbEntry.Property(x => x.UpdateDate).IsModified = true;
            dbEntry.Property(x => x.UpdateUserId).IsModified = true;
            return this;
        }


        public List<Email> GetEmailsByStatusAndUserId(EmailStatus status, long userId)
        {
            return _context.Emails.Where(x => x.Status.Equals(status) && x.CreationUserId == userId).ToList();
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
}
