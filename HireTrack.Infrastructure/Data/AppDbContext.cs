using HireTrack.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace HireTrack.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Candidate> Candidates => Set<Candidate>();
        public DbSet<Interview> Interviews => Set<Interview>();
        public DbSet<AppUser> Users => Set<AppUser>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Candidate>(e =>
            {
                e.HasKey(c => c.Id);
                e.Property(c => c.FullName).HasMaxLength(150).IsRequired();
                e.Property(c => c.Email).HasMaxLength(200).IsRequired();
                e.Property(c => c.Position).HasMaxLength(100).IsRequired();
                e.HasIndex(c => c.Email).IsUnique();
                e.HasIndex(c => c.Status);  // for status-based filtering queries
            });

            modelBuilder.Entity<Interview>(e =>
            {
                e.HasKey(i => i.Id);
                e.HasOne(i => i.Candidate)
                    .WithMany(c => c.Interviews)
                    .HasForeignKey(i => i.CandidateId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasIndex(i => i.ScheduledAt);
            });

            modelBuilder.Entity<AppUser>(e =>
            {
                e.HasKey(u => u.Id);
                e.HasIndex(u => u.Email).IsUnique();
                e.Property(u => u.Username).HasMaxLength(100).IsRequired();
            });
        }
    }
}
