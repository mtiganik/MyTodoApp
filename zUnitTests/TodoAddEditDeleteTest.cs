using Domain;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zUnitTests
{
    [TestFixture]
    public class TodoAddEditDeleteTest
    {
        private AppDbContext _context;
        private TodoService _service;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var options = SetupInMemoryDb.Setup("Test_Add_Edit_Delete", true);
            _context = new AppDbContext(options);
            _service = new TodoService(_context);
        }

        [OneTimeTearDown]
        public void OneTimeTeardown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task Add_Edit_Delete_Workflow()
        {
            // 1. succesfull creation
            var todoTask = await _service.CreateAsync(todo1);

            // Assert that the task was succesfully created and has a valid ID
            Assert.NotNull(todoTask);
            Assert.That(todoTask.Id, Is.Not.EqualTo(Guid.Empty));

            // Assert that the task exists in the database 
            var createdTask = await _service.GetByIdAsync(todoTask.Id);
            Assert.That(createdTask, Is.Not.Null);
            Assert.That(createdTask.Title, Is.EqualTo(todo1.Title));

            // 2. Edit some values in that task
            todoTask.DueDate = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            todoTask.Status = Status.Completed;

            // Updating can be with past time 
            var updatedTask = await _service.UpdateAsync(todoTask);
            Assert.That(updatedTask, Is.Not.Null);
            Assert.That(updatedTask.DueDate, Is.EqualTo(new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc)));
            Assert.That(updatedTask.Status, Is.EqualTo(Status.Completed));

            // 3. Delete the task
            await _service.DeleteAsync(todoTask.Id);

            // Assert that the task was succesfully deleted
            var deletedTask = await _service.GetByIdAsync(todoTask.Id);
            Assert.That(deletedTask, Is.Null);
        }



        [Test]
        public void DueDate_Past_Will_Throw_Error()
        {
            var exception = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await _service.CreateAsync(todo2);
            });

            Assert.That(exception.Message, Is.EqualTo("DueDate cannot be in the past"));
        }


        Todo todo1 = new Todo()
        {
            Title = "task with future date",
            Description = "bla bla bla",
            DueDate = new DateTime(2050, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            Status = Status.InProgress,
        };

        static Todo todo2 = new Todo()
        {
            Title = "task with past date",
            Description = "bla bla bla",
            DueDate = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            Status = Status.Pending,
        };

    }
}
