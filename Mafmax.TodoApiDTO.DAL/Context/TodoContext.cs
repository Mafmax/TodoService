using Mafmax.TodoApi.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mafmax.TodoApi.DAL.Context
{
    /// <summary>
    /// Контекст БД для TodoApi приложения
    /// </summary>
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        /// <summary>
        /// Список задач
        /// </summary>
        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
