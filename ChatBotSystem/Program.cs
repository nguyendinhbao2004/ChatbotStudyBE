using ChatBotInterfacture.Config;
using ChatBotSystem.Middlewares;
using Microsoft.EntityFrameworkCore;
using Pgvector.EntityFrameworkCore; // <--- THÊM DÒNG NÀY VÀO

namespace ChatBotSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString, o =>
                {
                    o.MigrationsAssembly("ChatBotInterfacture");
                    o.UseVector(); 
                }));

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            var app = builder.Build();

            app.UseMiddleware<GlobalExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}