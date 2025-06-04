using LibrarySystem.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibrarySystem.Common
{
    public class AsyncCrudService<T> : ICrudServiceAsync<T> where T : class
    {
        private readonly IRepository<T> _repository;

        public AsyncCrudService(IRepository<T> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<bool> CreateAsync(T element)
        {
            try
            {
                await _repository.AddAsync(element);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<T> ReadAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<T>> ReadAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            var allItems = await _repository.GetAllAsync();
            return allItems.Skip((page - 1) * amount).Take(amount);
        }

        public async Task<bool> UpdateAsync(T element)
        {
            try
            {
                await _repository.UpdateAsync(element);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveAsync(T element)
        {
            try
            {
                await _repository.DeleteAsync(element);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SaveAsync()
        {
            return true;
        }
    }
}