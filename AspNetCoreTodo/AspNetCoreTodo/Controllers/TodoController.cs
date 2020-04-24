using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreTodo.Services;
using AspNetCoreTodo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


namespace AspNetCoreTodo.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoItemService _todoItemService;
        private readonly UserManager<ApplicationUser> _userManager;

        public TodoController(ITodoItemService todoItemService, UserManager<ApplicationUser> userManager)
        {
            _todoItemService = todoItemService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index() 
        {
            // get currently logged in user
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            // get to-do items from db
            var items = await _todoItemService.GetIncompleteItemsAsync(currentUser);


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

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var success = await _todoItemService.AddItemAsync(newItem, currentUser);

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

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var success = await _todoItemService.MarkDoneAsync(id, currentUser);
            
            if (!success) 
            {
                return BadRequest("Could not mark item as done");
            } 

            return RedirectToAction("Index");
        }
    }
}