using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreTodo.Services;
using AspNetCoreTodo.Models;


namespace AspNetCoreTodo.Controllers
{
    public class TodoController : Controller
    {
        private readonly ITodoItemService _todoItemService;

        public TodoController(ITodoItemService todoItemService)
        {
            _todoItemService = todoItemService;
        }

        public async Task<IActionResult> Index() 
        {
            // get to-do items from db
            var items = await _todoItemService.GetIncompleteItemsAsync();


            // put items into model
            var model = new TodoViewModel()
            {
                Items = items
            };

            // render view using model
            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(TodoItem newItem)
        {
            if (!ModelState.IsValid)
            {
                // if model is invalid, redirect back to (todo) index
                return RedirectToAction("Index");
            }

            var success = await _todoItemService.AddItemAsync(newItem);

            if (!success)
            {
                return BadRequest("Could not add item.");
            }

            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkDone(Guid id) 
        {
            if (id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            var success = await _todoItemService.MarkDoneAsync(id);
            
            if (!success) 
            {
                return BadRequest("Could not mark item as done");
            } 

            return RedirectToAction("Index");
        }
    }
}