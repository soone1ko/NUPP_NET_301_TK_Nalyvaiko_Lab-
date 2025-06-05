using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibrarySystem.Common;

namespace LibrarySystem.Common
{
    public class CrudServiceAsync<T> : ICrudServiceAsync<T> where T : class
    {
        public Task<bool> CreateAsync(T element)
        {
            throw new NotImplementedException();
        }

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
