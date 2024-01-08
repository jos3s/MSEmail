using Microsoft.EntityFrameworkCore;
using MsEmail.Domain.Entities;
using MsEmail.Infra.Context;
using MSEmail.Common.Utils;
using MSEmail.Domain.Enums;
using MSEmail.Domain.Interfaces;
using MSEmail.Infra.Redis;
using MSEmail.Infra.Redis.CacheKeyGenerator;
using System.Linq.Expressions;

namespace MSEmail.Infra.Repository;

public class EmailRepository : IRepository<Email>, IResetCache
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
        ResetCache(entity);
        return this;
    }

    public IRepository<Email> Update(Email entity)
    {
        entity.UpdateDate = DateTime.Now;
        _context.Emails.Update(entity);
        ResetCache(entity);
        return this;
    }

    public IRepository<Email> Delete(long id)
    {
        try
        {
            Email email = _context.Emails.Find(id);
            email.DeletionDate = DateTime.Now;
            _context.Emails.Update(email);
            ResetCache(email);
            return this;
        }
        catch (Exception)
        {
            throw;
        }
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
        if (withDeletionDate) return _context.Emails.ToList();
        return _context.Emails.AsNoTracking().Where(x => x.DeletionDate == null).ToList();
    }

    public Email? GetById(long id)
    {
        if (ConfigHelper.UseCache)
        {
            var key = new EmailCacheKeyGenerator().GenerateKeyById(id);

            var email = RedisCache.GetValue<Email>(key);

            if (email.IsNull())
            {
                email = _context.Emails.FirstOrDefault(x => x.Id == id);
                if (!email.IsNull())
                    RedisCache.SetValue(key, email);
            }

            return email;
        }
        return _context.Emails.FirstOrDefault(x => x.Id == id);
    }

    public List<Email> GetEmailsByUserId(long userId, bool withDeletionDate = false)
    {
        if (withDeletionDate) return _context.Emails.Where(x => x.CreationUserId == userId).ToList();
        return _context.Emails.Where(x => x.DeletionDate == null && x.CreationUserId == userId).ToList();
    }

    public List<Email> Find(Expression<Func<Email, bool>> expression)
    {
        return _context.Emails.AsNoTracking().Where(expression).ToList();
    }

    public List<Email> GetEmailsByStatus(EmailStatus status)
    {
        if (ConfigHelper.UseCache)
        {
            List<Email> emails = new();

            var key = new EmailCacheKeyGenerator().GenerateKeyByStatus(status);

            emails = RedisCache.GetValue<List<Email>>(key);

            if (emails.IsNull())
            {
                emails = _context.Emails.AsNoTracking().Where(x => x.Status.Equals(status)).ToList();
                if (!emails.IsNull())
                    RedisCache.SetValue(key, emails);
            }

            return emails ?? new List<Email>();
        }

        return _context.Emails.AsNoTracking().Where(x => x.Status.Equals(status)).ToList();
    }

    public IRepository<Email> UpdateStatus(Email email)
    {
        email.UpdateDate = DateTime.Now;
        var dbEntry = _context.Entry(email);
        dbEntry.Property(x => x.Status).IsModified = true;
        dbEntry.Property(x => x.UpdateDate).IsModified = true;
        dbEntry.Property(x => x.UpdateUserId).IsModified = true;
        ResetCache(email);
        return this;
    }


    public List<Email> GetEmailsByStatusAndUserId(EmailStatus status, long userId)
    {
        if (ConfigHelper.UseCache)
        {
            List<Email> emails = new();

            var key = new EmailCacheKeyGenerator().GenerateKeyByStatusAndUserId(status, userId);

            emails = RedisCache.GetValue<List<Email>>(key);

            if (emails.IsNull())
            {
                emails = _context.Emails.AsNoTracking().Where(x => x.Status.Equals(status) && x.CreationUserId == userId).ToList();
                if (!emails.IsNull())
                    RedisCache.SetValue(key, emails, 48*60);
            }

            return emails ?? new List<Email>();
        }
        return _context.Emails.AsNoTracking().Where(x => x.Status.Equals(status) && x.CreationUserId == userId).ToList();
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

    public void ResetCache(object entity)
    {
        var email = (Email)entity;

        if (ConfigHelper.UseCache)
        {
            RedisCache.ClearKey(new EmailCacheKeyGenerator().GenerateKeyById(email.Id));
            if (!email.Status.IsNull())
            {
                RedisCache.ClearKey(new EmailCacheKeyGenerator().GenerateKeyByStatus(email.Status));
                if(!email.CreationUserId.IsNull()) 
                    RedisCache.ClearKey(new EmailCacheKeyGenerator().GenerateKeyByStatusAndUserId(email.Status, email.CreationUserId));
            }

        }
    }
}