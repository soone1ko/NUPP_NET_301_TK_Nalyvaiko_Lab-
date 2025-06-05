using System;

namespace LibrarySystem.REST.Models
{
    public class JournalDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Volume { get; set; }
        public int Issue { get; set; }
        public string? Publisher { get; set; }
    }
}
