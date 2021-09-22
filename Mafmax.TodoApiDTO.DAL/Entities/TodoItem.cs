using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mafmax.TodoApi.DAL.Entities
{
    /// <summary>
    /// Элемент списка задач
    /// </summary>
    [Table("TodoItems")]
    public class TodoItem
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [Key]
        public long Id { get; set; }
       
        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }
       
        /// <summary>
        /// Флаг завершенности задачи
        /// </summary>
        [Column("CompliteFlag")]
        public bool IsComplete { get; set; }
      
        /// <summary>
        /// Системная информация
        /// </summary>
        [Column("SecretValue")]
        public string Secret { get; set; }
    }
}
