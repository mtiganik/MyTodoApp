using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace zUnitTests
{
    public class SetupInMemoryDb
    {
        /// <summary>
        /// Creates options to use InMemory database for testing. 
        /// </summary>
        /// <param name="DbName">Name of your database. Eg 'test database'</param>
        /// <param name="withTestData">Initialize database with some test data</param>
        /// <returns>Options to initialise EFCore.AppDbContext </returns>
        public static DbContextOptions<AppDbContext> Setup(string DbName, bool withTestData)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: DbName)
                .Options;
            if (withTestData)
            {
                var todos = GetTestTodos();
                using (var context = new AppDbContext(options))
                {
                    foreach (var todo in todos) { context.Todos.Add(todo); }
                    context.SaveChanges();
                }
            }
            return options;
        }
        private static List<Todo> GetTestTodos()
        {
            string todosJson = File.ReadAllText("Data/todo-test-data.json");
            return JsonConvert.DeserializeObject<List<Todo>>(todosJson)!;
        }
    }
}
