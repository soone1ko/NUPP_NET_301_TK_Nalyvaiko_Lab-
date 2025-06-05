using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibrarySystem.Common; // <-- здесь лежит ICrudServiceAsync<T>

namespace LibrarySystem.Common
{
    public class AsyncCrudService<T> : ICrudServiceAsync<T> where T : class
    {
        // Здесь вы можете держать ссылку на репозиторий или на DbContext, 
        // но так как класс лежит в LibrarySystem.Common, скорее всего 
        // реальную логику вы реализуете в проекте Infrastructure. 
        // Пока что можно сделать “заглушки” (throw new NotImplementedException()), 
        // чтобы избавиться от ошибок компиляции.
        public Task<bool> CreateAsync(T element)
        {
            throw new NotImplementedException();
        }

        // 🔴 ЭТОТ МЕТОД БЫЛ ОТСУТСТВУЮЩИМ, ДОБАВЬТЕ ИХ ВСЕ НИЖЕ:
        public Task<T> ReadAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> ReadAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(T element)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(T element)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
