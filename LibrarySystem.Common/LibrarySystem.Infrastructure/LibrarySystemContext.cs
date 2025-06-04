using Microsoft.EntityFrameworkCore;
using LibrarySystem.Infrastructure.Models;

namespace LibrarySystem.Infrastructure
{
    public class LibrarySystemContext : DbContext
    {
        public LibrarySystemContext(DbContextOptions<LibrarySystemContext> options) : base(options) { }

        public DbSet<KnyhaModel> Knyhy { get; set; }
        public DbSet<LibraryModel> Libraries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KnyhaModel>()
                .HasOne(k => k.Library)
                .WithMany(l => l.Knyhy)
                .HasForeignKey(k => k.LibraryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}