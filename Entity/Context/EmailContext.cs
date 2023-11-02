using Microsoft.EntityFrameworkCore;
using Plataform.Models;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace Plataform.Context
{
    public class EmailContext : DbContext
    {
        public DbSet<EmailModel> Emails { get; set; } = null;

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer("Server=DESKTOP-2MB2599;Database=ms_email;User ID=sa;Password=201201;Trusted_Connection=False; TrustServerCertificate=True;");
    }
}
