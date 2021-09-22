using Mafmax.TodoApi.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mafmax.TodoApi.BLL.Services
{
    /// <summary>
    /// Определяет методы для работы со списком задач
    /// </summary>
   public interface ITodoService
    {
        /// <summary>
        /// Получает список задач
        /// </summary>
        /// <returns></returns>
        IQueryable<TodoItemDTO> GetTodoItems();

        /// <summary>
        /// Получает задачу по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TodoItemDTO> GetTodoItemAsync(long id);

        /// <summary>
        /// Обновляет значения элемента в БД
        /// </summary>
        /// <param name="itemDTO"></param>
        void UpdateTodoItem(TodoItemDTO itemDTO);

        /// <summary>
        /// Асинхронно обновляет значения элемента в БД
        /// </summary>
        /// <param name="itemDTO"></param>
        Task<int> UpdateTodoItemAsync(TodoItemDTO itemDTO);

        /// <summary>
        /// Асинхронно создает новый элемент задачу
        /// </summary>
        /// <param name="itemDTO"></param>
        Task<TodoItemDTO> CreateTodoItemAsync(TodoItemDTO itemDTO);

        /// <summary>
        /// Асинхронно удаляет задачу из хранилища
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteTodoItemAsync(long id);

        /// <summary>
        /// Проверят наличие 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool TodoItemExists(long id) =>
            GetTodoItems().Any(x => x.Id.Equals(id));
    }
}
