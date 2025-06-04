using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LibrarySystem.Infrastructure
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<LibrarySystemContext>
    {
        public LibrarySystemContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LibrarySystemContext>();
            optionsBuilder.UseSqlite("Data Source=library.db");

            return new LibrarySystemContext(optionsBuilder.Options);
        }
    }
}