using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using LibrarySystem.Common;

namespace LibrarySystem.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var bookService = new AsyncCrudService<Book>(b => b.Id);

            Console.WriteLine("Создание 1000 книг параллельно...");

            var tasks = new List<Task>();
            var semaphore = new SemaphoreSlim(20); // ограничиваем кол-во потоков

            for (int i = 0; i < 1000; i++)
            {
                await semaphore.WaitAsync();
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        var book = Book.CreateNew();
                        await bookService.CreateAsync(book);
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }));
            }

            await Task.WhenAll(tasks);
            Console.WriteLine("✅ Книги успешно созданы!");

            var allBooks = await bookService.ReadAllAsync();

            Console.WriteLine($"\n📊 Минимальный год: {allBooks.Min(b => b.Год)}");
            Console.WriteLine($"📊 Максимальный год: {allBooks.Max(b => b.Год)}");
            Console.WriteLine($"📊 Средний год: {allBooks.Average(b => b.Год)}");

            Console.WriteLine("\n📄 Пагинация (первая страница, по 5 книг):");
            var page1 = await bookService.ReadAllAsync(1, 5);
            foreach (var book in page1)
            {
                Console.WriteLine(book.ToString());
            }

            await bookService.SaveAsync();
            Console.WriteLine("\n💾 Коллекция сохранена в файл storage.json");

            // AutoResetEvent — пример
            var resetEvent = new AutoResetEvent(false);
            Console.WriteLine("\nНажмите Enter, чтобы продолжить и увидеть сообщение из другого потока...");
            Console.ReadLine();

            Task.Run(() =>
            {
                resetEvent.WaitOne(); // ждёт сигнала
                Console.WriteLine("🔔 Поток: получен сигнал и выполнено действие.");
            });

            Thread.Sleep(1000);
            resetEvent.Set(); // подаём сигнал

            Console.WriteLine("\nНажмите любую клавишу для завершения...");
            Console.ReadKey();
        }
    }
}
