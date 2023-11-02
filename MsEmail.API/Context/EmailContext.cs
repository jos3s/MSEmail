using Microsoft.EntityFrameworkCore;
using MsEmail.API.Entities;

namespace MsEmail.API.Context
{
    public class EmailContext : DbContext
    {
        public DbSet<Email> Emails { get; set; }

        public EmailContext(DbContextOptions<EmailContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Email>(e =>
            {
                e.HasKey(email => email.Id);

                e.Property(email => email.Body)
                .HasColumnType("nvarchar(max)");

                e.Property(email => email.DeletionDate).IsRequired(false);
            });


        }
    }
}
