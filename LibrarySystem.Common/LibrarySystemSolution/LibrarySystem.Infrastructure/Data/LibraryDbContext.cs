using System;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Journal> Journals { get; set; }
    }

    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public string? Description { get; set; }
    }

    public class Journal
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Volume { get; set; }
        public int Issue { get; set; }
        public string? Publisher { get; set; }
    }
}
