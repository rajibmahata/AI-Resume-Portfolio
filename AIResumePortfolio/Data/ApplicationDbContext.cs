using AIResumePortfolio.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AIResumePortfolio.Data
{
    // ApplicationDbContext.cs
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Donation> Donations { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //    => options.UseSqlite("Data Source=portfolios.db");

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlite("Data Source=portfolios.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           // modelBuilder.Entity<Donation>().HasNoKey();
        }
    }
}
