using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace LibrarySystem.Common
{
    public class AsyncCrudService<T> : ICrudServiceAsync<T> where T : class
    {
        private readonly ConcurrentDictionary<Guid, T> _storage = new();
        private readonly Func<T, Guid> _getId;
        private readonly SemaphoreSlim _fileSemaphore = new(1, 1); // для сохранения файла
        private readonly object _lock = new(); // для защиты операций с коллекцией

        public AsyncCrudService(Func<T, Guid> getId)
        {
            _getId = getId;
        }

        public async Task<bool> CreateAsync(T element)
        {
            var id = _getId(element);
            lock (_lock)
            {
                return _storage.TryAdd(id, element);
            }
        }

        public async Task<T> ReadAsync(Guid id)
        {
            _storage.TryGetValue(id, out var value);
            return value;
        }

        public async Task<IEnumerable<T>> ReadAllAsync()
        {
            return _storage.Values.ToList();
        }

        public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            return _storage.Values
                .Skip((page - 1) * amount)
                .Take(amount)
                .ToList();
        }

        public async Task<bool> UpdateAsync(T element)
        {
            var id = _getId(element);
            lock (_lock)
            {
                _storage[id] = element;
                return true;
            }
        }

        public async Task<bool> RemoveAsync(T element)
        {
            var id = _getId(element);
            lock (_lock)
            {
                return _storage.TryRemove(id, out _);
            }
        }

        public async Task<bool> SaveAsync()
        {
            await _fileSemaphore.WaitAsync();
            try
            {
                var json = JsonSerializer.Serialize(_storage.Values, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                await File.WriteAllTextAsync("storage.json", json);
                return true;
            }
            finally
            {
                _fileSemaphore.Release();
            }
        }

        public IEnumerator<T> GetEnumerator() => _storage.Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
