using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gistics.Models;

namespace Gistics.Services
{
    public interface ITodoService
    {
        Task<List<TodoItem>> GetAllAsync();
        Task<TodoItem?> GetAsync(Guid id);
        Task<TodoItem> CreateAsync(TodoItem item);
        Task<bool> UpdateAsync(TodoItem item);
        Task<bool> DeleteAsync(Guid id);
    }
}