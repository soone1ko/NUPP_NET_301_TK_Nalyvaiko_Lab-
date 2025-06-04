using System;
using LibrarySystem.Common;

namespace LibrarySystem.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var bookService = new CrudService<Book>();
            var kniga = new Book("Гиперплания на C#", 2023, "Джон Доу");
            bookService.Create(kniga);
            Console.WriteLine("Книги:");
            foreach (var book in bookService.ReadAll())
            {
                Console.WriteLine(book.ToString());
            }
            Console.WriteLine("Нажмите любую клавишу, чтобы завершить...");
            Console.ReadKey();
        }
    }
}