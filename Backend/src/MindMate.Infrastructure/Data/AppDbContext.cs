using Microsoft.EntityFrameworkCore;
using MindMate.Core.Entities;

namespace MindMate.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<JournalEntry> JournalEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasMany(u => u.JournalEntries)
                .WithOne(j => j.User)
                .HasForeignKey(j => j.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure JournalEntry entity
            modelBuilder.Entity<JournalEntry>()
                .HasKey(j => j.Id);

            modelBuilder.Entity<JournalEntry>()
                .Property(j => j.MoodRating)
                .IsRequired();

            modelBuilder.Entity<JournalEntry>()
                .Property(j => j.EntryText)
                .IsRequired();
        }
    }
} 