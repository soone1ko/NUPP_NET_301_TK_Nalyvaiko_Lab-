using System;
using System.Collections.Generic;
using System.Linq;

namespace LibrarySystem.Common
{
    public interface ICrudService<T>
    {
        void Create(T element);
        T Read(Guid id);
        IEnumerable<T> ReadAll();
        void Update(T element);
        void Remove(T element);
    }

    public class CrudService<T> : ICrudService<T> where T : class
    {
        private List<T> _items = new List<T>();

        public void Create(T element)
        {
            _items.Add(element);
        }

        public T Read(Guid id)
        {
            return _items.FirstOrDefault(item => (item as dynamic).Id == id);
        }

        public IEnumerable<T> ReadAll()
        {
            return _items.AsReadOnly();
        }

        public void Update(T element)
        {
            var existing = Read((element as dynamic).Id);
            if (existing != null)
            {
                int index = _items.IndexOf(existing);
                _items[index] = element;
            }
        }

        public void Remove(T element)
        {
            _items.Remove(element);
        }
    }
}