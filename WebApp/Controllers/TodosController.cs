using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain;
using WebApp.ViewModels;
using Services;

namespace WebApp.Controllers
{
    public class TodosController : Controller
    {
        private readonly ITodoService _service;

        public TodosController(ITodoService service)
        {
            _service = service;
        }

        // GET: Todos
        public async Task<IActionResult> Index(string? searchKeyword, string? statusFilter, string? sortBy)
        {
            var todos = await _service.GetAllAsync(searchKeyword, statusFilter, sortBy);

            var viewModel = new TodoFilterViewModel
            {
                SearchKeyword = searchKeyword,
                StatusFilter = statusFilter,
                SortBy = sortBy,
                Todos = todos
            };
            return View(viewModel);
        }

        // GET: Todos/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _service.GetByIdAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        // GET: Todos/Create
        public IActionResult Create()
        {
            var model = new TodoViewModel
            {
                StatusList = Enum.GetValues(typeof(Status)).Cast<Status>()
                .Select(s => new SelectListItem
                {
                    Text = s.ToString(),
                    Value = s.ToString()
                }).ToList()
            };
            return View(model);
        }

        // POST: Todos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,DueDate,Status")] Todo todo)
        {
            if (ModelState.IsValid)
            {
                todo = await _service.CreateAsync(todo);
                return RedirectToAction(nameof(Index));
            }
            return View(todo);
        }

        // GET: Todos/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _service.GetByIdAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            var viewModel = new TodoViewModel
            {
                Todo = todo,
                StatusList = Enum.GetValues(typeof(Status))
                            .Cast<Status>()
                            .Select(s => new SelectListItem
                            {
                                Text = s.ToString(),
                                Value = s.ToString(),
                                Selected = s == todo.Status
                            })
            };
            return View(viewModel);
        }

        // POST: Todos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Description,DueDate,Status")] Todo todo)
        {
            if (id != todo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    todo = await _service.UpdateAsync(todo);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _service.TodoExistsAsync(todo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(todo);
        }

        // GET: Todos/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var todo = await _service.GetByIdAsync(id);

            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        // POST: Todos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                await _service.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

