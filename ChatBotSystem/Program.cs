using ChatBotApplication;
using ChatBotInterfacture;
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

            builder.Services.AddApplication();
            // 2. Đăng ký Layer Infrastructure (Repository, DB) -> THÊM DÒNG NÀY VÀO
            // Lưu ý: Phải truyền builder.Configuration vào để nó lấy ConnectionString
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer(); // ❗ bắt buộc
            builder.Services.AddSwaggerGen();
            var app = builder.Build();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}