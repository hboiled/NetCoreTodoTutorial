using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreTodo.Models;

namespace AspNetCoreTodo.Services
{
    public class FakeTodoItemService : ITodoItemService
    {
        public Task<TodoItem[]> GetIncompleteItemsAsync(ApplicationUser user)
        {
            var item1 = new TodoItem
            {
                Title = "Filler Entry",
                DueAt = DateTimeOffset.Now.AddDays(1)
            };

            var item2 = new TodoItem
            {
                Title = "Filler Entry2",
                DueAt = DateTimeOffset.Now.AddDays(2)
            };

            return Task.FromResult(new[] {item1, item2});
        }

        public Task<bool> AddItemAsync(TodoItem newItem, ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> MarkDoneAsync(Guid id, ApplicationUser user)
        {
            throw new NotImplementedException();
        }
    }
}