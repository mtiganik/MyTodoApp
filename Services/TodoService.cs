using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class TodoService : ITodoService
    {
        private AppDbContext _context;

        public TodoService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Todo> CreateAsync(Todo task)
        {
            if (task.DueDate < DateTime.Now)
            {
                throw new InvalidOperationException("DueDate cannot be in the past");
            }

            task.Id = Guid.NewGuid();
            task.DueDate = task.DueDate.ToUniversalTime();
            _context.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task DeleteAsync(Guid id)
        {
            var todo = await _context.Todos.FindAsync(id) ?? throw new ArgumentException("Task not found");
            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Todo>> GetAllAsync(string? searchKeyword, string? statusFilter, string? sortBy)
        {
            var tasksQuery = _context.Todos.AsQueryable();
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                var lowerSearch = searchKeyword.ToLower();
                tasksQuery = tasksQuery.Where(t => t.Title!.ToLower().Contains(lowerSearch)
                || t.Description!.ToLower().Contains(lowerSearch));
            }
            if (!string.IsNullOrEmpty(statusFilter))
            {
                if (Enum.TryParse(typeof(Status), statusFilter, out var status))
                {
                    tasksQuery = tasksQuery.Where(t => t.Status == (Status)status);
                }
            }
            switch (sortBy)
            {
                case "DueDate":
                    tasksQuery = tasksQuery.OrderBy(t => t.DueDate).ThenBy(t => t.Title);
                    break;
                case "Status":
                    tasksQuery = tasksQuery.OrderBy(t => t.Status).ThenBy(t => t.Title);
                    break;
                default:
                    tasksQuery = tasksQuery.OrderBy(t => t.Title);
                    break;
            }
            return await tasksQuery.ToListAsync();

        }

        public async Task<Todo?> GetByIdAsync(Guid? id)
        {
            if (id == null) return null;
            return await _context.Todos.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<bool> TodoExistsAsync(Guid id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null) { return false; }
            return true;
        }

        public async Task<Todo> UpdateAsync(Todo task)
        {
            task.DueDate = task.DueDate.ToUniversalTime();
            _context.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }
    }
}
