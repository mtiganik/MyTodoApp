using Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels
{
    public class TodoViewModel
    {
        public Todo Todo { get; set; } = default!;
        public IEnumerable<SelectListItem> StatusList { get; set; } = default!;
    }
}
