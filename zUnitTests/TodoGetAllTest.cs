using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Services;

namespace zUnitTests
{
    [TestFixture]
    public class TodoGetAllTest
    {
        private AppDbContext _context;
        private TodoService _service;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var options = SetupInMemoryDb.Setup("Test_Get_Method", true);
            _context = new AppDbContext(options);
            _service = new TodoService(_context);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _context.Dispose();
        }

        [Test]
        [TestCase("", 5, TestName = "Empty keyword should return all values")]
        [TestCase("Entity", 5, TestName = "Test keyword in title")]
        [TestCase("foo", 2, TestName = "Case-insensitive lovercase search")]
        [TestCase("FOO", 2, TestName = "Case-insensitive uppercase search")]
        public async Task GetAll_By_SearchKeyword(string searchKeyword, int expectedResult)
        {
            var result = await _service.GetAllAsync(searchKeyword, null, null);

            Assert.That(result.Count(), Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("InProgress", 2, TestName = "Filter InProgress")]
        [TestCase("Pending", 2, TestName = "Filter Pending")]
        [TestCase("Completed", 1, TestName = "Filter Completed")]
        public async Task GetAll_By_StatusFilter(string statusFiler, int expectedResult)
        {
            var result = await _service.GetAllAsync(null, statusFiler, null);

            Assert.That(result.Count, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("Status", "Entity1", "Entity3", TestName = "Sort by status")]
        [TestCase("DueDate", "Entity3", "Entity1", TestName = "Sort by due date")]
        public async Task GetAll_By_SortBy(string sortBy, string firstResultTitle, string thirdResultTitle)
        {
            IEnumerable<Todo> result = await _service.GetAllAsync(null, null, sortBy);

            Assert.That(result.ToList()[0].Title, Is.EqualTo(firstResultTitle));
            Assert.That(result.ToList()[2].Title, Is.EqualTo(thirdResultTitle));
        }

    }
}
