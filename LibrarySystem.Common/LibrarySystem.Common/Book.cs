using System;

namespace LibrarySystem.Common
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Название { get; set; }
        public int Год { get; set; }
        public string Автор { get; set; }

        public Book(string название, int год, string автор)
        {
            Id = Guid.NewGuid();
            Название = название;
            Год = год;
            Автор = автор;
        }

        public override string ToString()
        {
            return $"Книга: {Название}, Автор: {Автор}, Год: {Год}";
        }
    }
}