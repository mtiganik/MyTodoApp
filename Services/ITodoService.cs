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
        /// <summary>
        /// Retrieves a todo by its unique identifier
        /// </summary>
        /// <param name="id">The unique identifier for the TodoTask</param>
        /// <returns>Correct Todotask, or null if not found</returns>
        Task<Todo?> GetByIdAsync(Guid? id);

        /// <summary>
        /// Returns all Todos that correspond to search parameters
        /// </summary>
        /// <param name="searchKeyword">KeyWord to search for in 'Title' or 'Description' value. Leave empty if not needed </param>
        /// <param name="statusFilter">Accepting words are: Pending, InProgress, Completed. Otherwise no filtering </param>
        /// <param name="sortBy">Accepts 'DueDate' and 'Status' for diferent sorting. Otherwise sorts by Title </param>
        /// <returns>A list of TodoTask, leave search/filter parameters empty if you want all todos</returns>
        Task<IEnumerable<Todo>> GetAllAsync(string? searchKeyword, string? statusFilter, string? sortBy);

        /// <summary>
        /// Deletes an Todo. Throws error if no todo are found
        /// </summary>
        /// <param name="id">The unique identifier for the Todo</param>
        /// <returns></returns>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Checks wheter Todo exists in database
        /// </summary>
        /// <param name="id">The unique identifier for the Todo</param>
        /// <returns>'True' if exists and 'false' if it does not</returns>

        Task<bool> TodoExistsAsync(Guid id);

        /// <summary>
        /// Creates new todo. Generates Id for todo and converts its DueTime to UTC time  
        /// </summary>
        /// <param name="task">Todo with Title, Description, Status and DueDate values</param>
        /// <returns>Todo with values retrived from database</returns>
        Task<Todo> CreateAsync(Todo task);

        /// <summary>
        /// Updates current todo
        /// </summary>
        /// <param name="task">Todo to update</param>
        /// <returns>Updated todo</returns>
        Task<Todo> UpdateAsync(Todo task);
    }
}
