using ChatBotApplication;
using ChatBotInterfacture;
using ChatBotInterfacture.Config;
using ChatBotSystem.Middlewares;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
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
            
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "ChatBot API",
                    Version = "v1"
                });
                // 1. Định nghĩa Security Scheme (Cấu hình nút Authorize)
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Vui lòng nhập Token vào ô bên dưới (Không cần chữ 'Bearer ' ở đầu)",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                // 2. Yêu cầu bảo mật (Áp dụng cho toàn bộ API)
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

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
            app.UseAuthentication();
            app.MapControllers();

            app.Run();
        }
    }
}