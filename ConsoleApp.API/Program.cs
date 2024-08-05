using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace ConsoleApi
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices(services =>
                    {
                        services.AddControllers().AddNewtonsoftJson();

                        // Configurar o Swagger
                        services.AddSwaggerGen(c =>
                        {
                            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Console API", Version = "v1" });
                        });
                    });

                    webBuilder.Configure(app =>
                    {
                        //if (app.Environment.IsDevelopment())
                        //{
                        //    app.UseDeveloperExceptionPage();
                        //}

                        // Ativar o middleware para servir o Swagger gerado como um JSON endpoint.
                        app.UseSwagger();

                        // Ativar o middleware para servir o Swagger UI (HTML, JS, CSS, etc.),
                        // especificando o endpoint Swagger JSON.
                        app.UseSwaggerUI(c =>
                        {
                            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Console API V1");
                            c.RoutePrefix = string.Empty; // Para acessar o Swagger UI em raiz (http://localhost:<porta>/)
                        });

                        app.UseRouting();

                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();
                        });
                    });
                });
    }

    public class TimeController : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        [HttpGet("gettime")]
        public IActionResult GetTime()
        {
            return Ok(new { Time = DateTime.Now });
        }

        [HttpGet("greet/{name}")]
        public IActionResult Greet(string name)
        {
            return Ok(new { Message = $"Hello, {name}!" });
        }
    }
}
