using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Services
{
    public interface ITodoService
    {
        Task<Todo?> GetByIdAsync(Guid? id);
        Task<IEnumerable<Todo>> GetAllAsync(string? searchKeyword, string? statusFilter, string? sortBy);
        Task DeleteAsync(Guid id);
        Task<bool> TodoExistsAsync(Guid id);
        Task<Todo> CreateAsync(Todo task);
        Task<Todo> UpdateAsync(Todo task);
    }
}
