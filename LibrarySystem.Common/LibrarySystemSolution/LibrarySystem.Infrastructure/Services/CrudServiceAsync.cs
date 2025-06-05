using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibrarySystem.Common;
using LibrarySystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Services
{
    public class CrudServiceAsync<T> : ICrudServiceAsync<T> where T : class
    {
        private readonly LibraryDbContext _context;

        public CrudServiceAsync(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(T element)
        {
            _context.Set<T>().Add(element);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<T> ReadAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> ReadAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            if (page <= 0) page = 1;
            if (amount <= 0) amount = 10;

            return await _context.Set<T>()
                                 .Skip((page - 1) * amount)
                                 .Take(amount)
                                 .ToListAsync();
        }

        public async Task<bool> UpdateAsync(T element)
        {
            _context.Set<T>().Update(element);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAsync(T element)
        {
            _context.Set<T>().Remove(element);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SaveAsync()
        {
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
