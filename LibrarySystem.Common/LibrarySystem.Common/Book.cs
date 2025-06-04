using System;

namespace LibrarySystem.Common
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Название { get; set; }
        public int Год { get; set; }
        public string Автор { get; set; }

        // Пустой конструктор для сериализации и Parallel.For
        public Book()
        {
            Id = Guid.NewGuid();
        }

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

        // 🔧 Метод для генерации случайной книги (нужно для ЛР №2)
        public static Book CreateNew()
        {
            var random = new Random();
            var authors = new[] { "Тарас Шевченко", "Ліна Костенко", "Лев Толстой", "Франс Кафка", "Дж. Р. Р. Толкін" };
            var titles = new[] { "Таємниці лісу", "Космічні пригоди", "Історія душі", "Втеча в безодню", "Книга ночі" };

            return new Book
            {
                Название = titles[random.Next(titles.Length)],
                Автор = authors[random.Next(authors.Length)],
                Год = random.Next(1900, 2025)
            };
        }
    }
}
