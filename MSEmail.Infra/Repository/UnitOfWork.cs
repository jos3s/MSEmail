using MsEmail.Infra.Context;
using MSEmail.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEmail.Infra.Repository
{
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
}
