using Microsoft.EntityFrameworkCore;
using MsEmail.Domain.Entities;
using MsEmail.Domain.Entities.Common;

namespace MsEmail.Infra.Context;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Email> Emails { get; set; }
    public DbSet<SystemLog> SystemLogs { get; set; }    
    public DbSet<ExceptionLog> ExceptionLogs { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(user => user.Id);

            e.Property(user => user.DeletionDate).IsRequired(false);
        });

        modelBuilder.Entity<Email>(e =>
        {
            e.HasKey(email => email.Id);

            e.Property(email => email.Body)
                .HasColumnType("nvarchar(max)");

            e.Property(email => email.DeletionDate).IsRequired(false);
        });

        modelBuilder.Entity<SystemLog>(e =>
        {
            e.HasKey(log => log.Id);

            e.Property(log => log.DeletionDate).IsRequired(false);
        });
            
        modelBuilder.Entity<ExceptionLog>(e =>
        {
            e.HasKey(exception => exception.Id);

            e.Property(exception => exception.DeletionDate).IsRequired(false);
        });
    }
}