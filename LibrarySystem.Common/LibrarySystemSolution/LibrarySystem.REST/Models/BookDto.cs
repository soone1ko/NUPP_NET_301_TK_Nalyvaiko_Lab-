using System;

namespace LibrarySystem.REST.Models
{
    public class BookDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public string? Description { get; set; }
    }
}
