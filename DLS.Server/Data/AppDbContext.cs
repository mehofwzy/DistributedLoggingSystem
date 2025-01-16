using DLS.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace DLS.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<LogEntry> LogEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogEntry>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Service).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Level).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Message).IsRequired();
                entity.Property(e => e.Timestamp).IsRequired();
                entity.Property(e => e.BackendType).IsRequired().HasMaxLength(50);

            });
        }
    }
}
