using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gistics.Models;

namespace Gistics.Services
{
    // Simple thread-safe in-memory CRUD store. Replace with EF / DB implementation as needed.
    public class TodoService : ITodoService
    {
        private readonly ConcurrentDictionary<Guid, TodoItem> _store = new();

        public Task<List<TodoItem>> GetAllAsync()
        {
            var list = _store.Values.OrderByDescending(t => t.CreatedAt).ToList();
            return Task.FromResult(list);
        }

        #region othermethods
        public Task<TodoItem?> GetAsync(Guid id)
        {
            _store.TryGetValue(id, out var item);
            return Task.FromResult(item);
        }

        public Task<TodoItem> CreateAsync(TodoItem item)
        {
            item.Id = Guid.NewGuid();
            item.CreatedAt = DateTime.UtcNow;
            _store[item.Id] = item;
            return Task.FromResult(item);
        }

        public Task<bool> UpdateAsync(TodoItem item)
        {
            if (item.Id == Guid.Empty) return Task.FromResult(false);
            if (!_store.ContainsKey(item.Id)) return Task.FromResult(false);
            _store[item.Id] = item;
            return Task.FromResult(true);
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            return Task.FromResult(_store.TryRemove(id, out _));
        }
        #endregion
    }
}