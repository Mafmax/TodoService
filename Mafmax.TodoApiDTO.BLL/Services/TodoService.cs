using Mafmax.TodoApi.BLL.DTOs;
using Mafmax.TodoApi.DAL.Context;
using Mafmax.TodoApi.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mafmax.TodoApi.BLL.Services
{
    /// <summary>
    /// Класс для обработки данных
    /// </summary>
    public class TodoService : ITodoService
    {
        protected readonly TodoContext db;

        public TodoService(TodoContext db)
        {
            this.db = db;
        }
       
        /// <summary>
        /// Получает список задач
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<TodoItemDTO> GetTodoItems() =>
            db.TodoItems.Select(x => TodoItemDTO.ItemToDTO(x));
       
        /// <summary>
        /// Получает задачу по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<TodoItemDTO> GetTodoItemAsync(long id)
        {
            TodoItem item = await db.TodoItems.FindAsync(id);
            if (item is null) return null;
            return TodoItemDTO.ItemToDTO(item);
        }
       
        /// <summary>
        /// Обновляет значения элемента в БД
        /// </summary>
        /// <param name="itemDTO"></param>
        public virtual void UpdateTodoItem(TodoItemDTO itemDTO)
        {
            db.TodoItems.Update(itemDTO.GetItem());
            db.SaveChanges();
        }

        /// <summary>
        /// Асинхронно обновляет значения элемента в БД
        /// </summary>
        /// <param name="itemDTO"></param>
        public virtual async Task<int> UpdateTodoItemAsync(TodoItemDTO itemDTO)
        {
            db.TodoItems.Update(itemDTO.GetItem());
            return await db.SaveChangesAsync();
        }
       
        /// <summary>
        /// Асинхронно создает новый элемент задачу
        /// </summary>
        /// <param name="itemDTO"></param>
        public virtual async Task<TodoItemDTO> CreateTodoItemAsync(TodoItemDTO itemDTO)
        {
            var item = itemDTO.GetItem();
            db.TodoItems.Add(item);
            await db.SaveChangesAsync();
            return TodoItemDTO.ItemToDTO(item);

        }
      
        /// <summary>
        /// Асинхронно удаляет задачу из хранилища
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<int> DeleteTodoItemAsync(long id)
        {
            var item = await db.TodoItems.FindAsync(id);
            db.TodoItems.Remove(item);
            return await db.SaveChangesAsync();
        }
      
        /// <summary>
        /// Проверят наличие 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool TodoItemExists(long id) =>
            GetTodoItems().Any(x => x.Id.Equals(id));
    }
}
