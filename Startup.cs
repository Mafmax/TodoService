using Mafmax.TodoApi.BLL.Services;
using Mafmax.TodoApi.DAL.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.IO;
using System.Reflection;
#pragma warning disable CS1591
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

            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "Todo API",
                    Version = "v1",
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                opt.IncludeXmlComments(xmlPath);
            });

            services.AddControllers(opt =>
            {
                opt.OutputFormatters.Clear();
                opt.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "TODO WEB API");
                opt.RoutePrefix = string.Empty;

            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseSerilogRequestLogging();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
