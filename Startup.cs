using Mafmax.TodoApi.BLL.Services;
using Mafmax.TodoApi.DAL.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace TodoApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public Action<DbContextOptionsBuilder> GetInMemoryBuilder(string name)
        {
            return opt => opt.UseInMemoryDatabase(name);
        }
        public Action<DbContextOptionsBuilder> GetMsSqlBuilder(string name)
        {
            var connectionString = Configuration.GetConnectionString(name);
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception($"Connection string {name} not found");
            }
            return opt => opt.UseSqlServer(connectionString);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionName = "TodoList";
            //var dbBuilder = GetInMemoryBuilder(connectionName);
            var dbBuilder = GetMsSqlBuilder(connectionName);
            services.AddDbContext<TodoContext>(dbBuilder);
            services.AddScoped<ITodoService, TodoService>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
