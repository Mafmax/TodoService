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
        private readonly ILogger<TodoItemsController> logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public TodoItemsController(ITodoService service, ILogger<TodoItemsController> logger)
        {

            this.service = service;
            this.logger = logger;
        }
        /// <summary>
        /// Gets the list of all todo items
        /// </summary>
        /// <returns>List of TodoItems</returns>
        /// <response code="200">Returns list of TodoItems</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
            return await service.GetTodoItems().ToListAsync();
        }
        /// <summary>
        /// Gets todo item by id
        /// </summary>
        /// <param name="id">TodoItem ID</param>
        /// <returns>Found item</returns>
        /// <response code="200">Returns found TodoItems</response>
        /// <response code="404">If TodoItem not found.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
            var todoItem = await service.GetTodoItemAsync(id);

            if (todoItem == null)
            {
                logger.LogWarning("TodoItem not found. Id was {ID}", id);
                return NotFound();
            }

            return todoItem;
        }
        /// <summary>
        /// Updates todo item in storage
        /// </summary>
        /// <param name="id">TodoItem ID</param>
        /// <param name="todoItemDTO">Item to update</param>
        /// <returns>No content</returns>
        /// <response code="204">If TodoItem updated successfully</response>
        /// <response code="400">If IDs do not matching</response>
        /// <response code="404">If TodoItem not found</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItem(long id, TodoItemDTO todoItemDTO)
        {
            if (id != todoItemDTO.Id)
            {
                logger.LogWarning("Update TodoItem fails. IDs dont match. Id was {id}, TodoItem.Id was {TodoItem.Id}", id, todoItemDTO.Id);
                return BadRequest();
            }

            var todoItem = await service.GetTodoItemAsync(id);
            if (todoItem == null)
            {
                logger.LogWarning("TodoItem not found. Id was {ID}", id);
                return NotFound();
            }

            try
            {
                await service.UpdateTodoItemAsync(todoItemDTO);
            }
            catch (DbUpdateConcurrencyException ex) when (!service.TodoItemExists(id))
            {
                logger.LogError(ex, "An exception was thrown");
                return NotFound();
            }

            return NoContent();
        }
        /// <summary>
        /// Create todo item in storage
        /// </summary>
        /// <param name="todoItemDTO">Item to create</param>
        /// <returns>Newly created TodoItem</returns>
        /// <response code="201">Item created successfully</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<ActionResult<TodoItemDTO>> CreateTodoItem(TodoItemDTO todoItemDTO)
        {
            var createdItem = await service.CreateTodoItemAsync(todoItemDTO);
            return CreatedAtAction(
                nameof(GetTodoItem),
                new { id = createdItem.Id },
                createdItem);
        }


        /// <summary>
        /// Remove element from storage
        /// </summary>
        /// <param name="id">TodoItem id</param>
        /// <returns>No content</returns>
        /// <response code="204">TodoItem deleted successfully</response>
        /// <response code="404">TodoItem not found. Make sure the ID is correct</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await service.GetTodoItemAsync(id);

            if (todoItem == null)
            {
                logger.LogWarning("TodoItem not found. Id was {ID}", id);
                return NotFound();
            }

            await service.DeleteTodoItemAsync(id);

            return NoContent();
        }
    }
}
