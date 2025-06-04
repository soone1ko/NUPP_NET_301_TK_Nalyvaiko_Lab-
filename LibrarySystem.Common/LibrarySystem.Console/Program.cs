using LibrarySystem.Common;
using LibrarySystem.Infrastructure;
using LibrarySystem.Infrastructure.Models;
using LibrarySystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Text;

namespace LibrarySystem.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Настройка кодировки консоли для корректного отображения кириллицы
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            try
            {
                // 1. Определяем путь к БД
                var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "library.db");
                Console.WriteLine($"Путь к файлу БД: {dbPath}");

                // 2. Настраиваем подключение (без избыточного логирования)
                var options = new DbContextOptionsBuilder<LibrarySystemContext>()
                    .UseSqlite($"Data Source={dbPath}")
                    .Options;

                // 3. Создаем/пересоздаем БД
                using (var context = new LibrarySystemContext(options))
                {
                    Console.WriteLine("Инициализация БД...");
                    await context.Database.EnsureDeletedAsync();
                    await context.Database.EnsureCreatedAsync();
                    Console.WriteLine("БД успешно инициализирована\n");

                    var repository = new Repository<KnyhaModel>(context);
                    var service = new AsyncCrudService<KnyhaModel>(repository);

                    // Добавляем библиотеку
                    var library = new LibraryModel { Nazva = "Центральна бібліотека" };
                    context.Libraries.Add(library);
                    await context.SaveChangesAsync();
                    Console.WriteLine($"Добавлена библиотека: {library.Nazva} (ID: {library.Id})");

                    // Добавляем книги
                    var books = new[]
                    {
                        new KnyhaModel
                        {
                            Nazva = "Гіперпланування на C#",
                            Rik = 2023,
                            Avtor = "Іван Іванов",
                            LibraryId = library.Id
                        },
                        new KnyhaModel
                        {
                            Nazva = "Мова програмування",
                            Rik = 2020,
                            Avtor = "Марія Петрова",
                            LibraryId = library.Id
                        }
                    };

                    foreach (var book in books)
                    {
                        await service.CreateAsync(book);
                    }
                    Console.WriteLine($"Добавлено {books.Length} книги\n");

                    // Выводим список книг
                    Console.WriteLine("Список книг:");
                    var allBooks = await service.ReadAllAsync();
                    foreach (var book in allBooks)
                    {
                        Console.WriteLine($"- {book.Nazva} ({book.Rik}), автор: {book.Avtor}");
                    }

                    // Статистика
                    Console.WriteLine($"\nВсего библиотек: {await context.Libraries.CountAsync()}");
                    Console.WriteLine($"Всего книг: {await context.Knyhy.CountAsync()}");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nОшибка: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Детали: {ex.InnerException.Message}");
                }
                Console.ResetColor();
            }
            finally
            {
                Console.WriteLine("\nНатисніть будь-яку клавішу, щоб завершити...");
                Console.ReadKey();
            }
        }
    }
}