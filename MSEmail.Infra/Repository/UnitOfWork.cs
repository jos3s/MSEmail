using MsEmail.Infra.Context;
using MSEmail.Domain.Interfaces;

namespace MSEmail.Infra.Repository;

public class UnitOfWork : IUnitOfWork
{
    private AppDbContext _context { get; set; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public void Commit()
    {
        _context.SaveChanges();
    }

    public void Rollback()
    {
    }
}