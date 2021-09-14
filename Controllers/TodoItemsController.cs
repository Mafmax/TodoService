using Mafmax.TodoApi.BLL.DTOs;
using Mafmax.TodoApi.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace TodoApi.Controllers
{
    /// <summary>
    /// Контроллер задач
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoService service;



        public TodoItemsController(ITodoService service)
        {

            this.service = service;
          
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
            return await service.GetTodoItems().ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
            var todoItem = await service.GetTodoItemAsync(id);

            if (todoItem == null)
            {
               
                return NotFound();
            }

            return todoItem;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItem(long id, TodoItemDTO todoItemDTO)
        {
            if (id != todoItemDTO.Id)
            {
          return BadRequest();
            }

            var todoItem = await service.GetTodoItemAsync(id);
            if (todoItem == null)
            {
               
                return NotFound();
            }

            try
            {
                await service.UpdateTodoItemAsync(todoItemDTO);
            }
            catch (DbUpdateConcurrencyException) when (!service.TodoItemExists(id))
            {

                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<TodoItemDTO>> CreateTodoItem(TodoItemDTO todoItemDTO)
        {
            var createdItem = await service.CreateTodoItemAsync(todoItemDTO);
            return CreatedAtAction(
                nameof(GetTodoItem),
                new { id = createdItem.Id },
                createdItem);
        }


     
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await service.GetTodoItemAsync(id);

            if (todoItem == null)
            {

                return NotFound();
            }

            await service.DeleteTodoItemAsync(id);

            return NoContent();
        }
    }
}
