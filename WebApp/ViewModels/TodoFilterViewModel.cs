using Domain;

namespace WebApp.ViewModels
{
    public class TodoFilterViewModel
    {
        public string? SearchKeyword { get; set; }
        public string? StatusFilter { get; set; }
        public string? SortBy { get; set; }
        public IEnumerable<Todo>? Todos { get; set; }
    }
}
