﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class TodosController : Controller
    {
        private readonly AppDbContext _context;

        public TodosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Todos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Todos.ToListAsync());
        }

        // GET: Todos/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.Todos
                .FirstOrDefaultAsync(m => m.Id == id);
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
                if(todo.DueDate < DateTime.Now)
                {
                    throw new InvalidOperationException("DueDate cannot be in the past.");
                }
                todo.Id = Guid.NewGuid();
                todo.DueDate = todo.DueDate.ToUniversalTime();
                _context.Add(todo);
                await _context.SaveChangesAsync();
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

            var todo = await _context.Todos.FindAsync(id);
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
                    todo.DueDate = todo.DueDate.ToUniversalTime();
                    _context.Update(todo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoExists(todo.Id))
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

            var todo = await _context.Todos
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var todo = await _context.Todos.FindAsync(id);
            if (todo != null)
            {
                _context.Todos.Remove(todo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TodoExists(Guid id)
        {
            return _context.Todos.Any(e => e.Id == id);
        }
    }
}




