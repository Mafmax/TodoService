using AutoMapper;
using Mafmax.TodoApi.DAL.Entities;

namespace Mafmax.TodoApi.BLL.DTOs
{
    /// <summary>
    /// Data Transfer Object для TodoItem
    /// </summary>
    public class TodoItemDTO
    {
        private static readonly Mapper toDTO;
        private static readonly Mapper fromDTO;
       
        static TodoItemDTO()
        {
            toDTO = new Mapper(new MapperConfiguration(cnf => cnf.CreateMap<TodoItem, TodoItemDTO>()));
            fromDTO = new Mapper(new MapperConfiguration(cnf => cnf.CreateMap<TodoItemDTO, TodoItem>()));
        }
       
        /// <summary>
        /// Получает транспорт из элемента
        /// </summary>
        /// <param name="item">Элемент</param>
        /// <returns></returns>
        public static TodoItemDTO ItemToDTO(TodoItem item)
        {
            return toDTO.Map<TodoItemDTO>(item);

        }
       
        /// <summary>
        /// Получает элемент из транспорта
        /// </summary>
        /// <returns></returns>
        public TodoItem GetItem()
        {
            return fromDTO.Map<TodoItem>(this);

        }

        /// <summary>
        /// Идентификатор задачи
        /// </summary>
        public long Id { get; set; }
       
        /// <summary>
        /// Название задачи
        /// </summary>
        public string Name { get; set; }
      
        /// <summary>
        /// Флаг завершенности задачи
        /// </summary>
        public bool IsComplete { get; set; }
    }
}
