using System.Text;
using System.Text.Json;
using ChatBotApplication;
using ChatBotApplication.Common.Interfaces;
using ChatBotInterfacture;
using ChatBotInterfacture.Config;
using ChatBotSystem.Middlewares;
using Domain.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Pgvector.EntityFrameworkCore; // <--- THÊM DÒNG NÀY VÀO
using System.Threading.Tasks;

namespace ChatBotSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
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
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
.AddJwtBearer(options =>
{
    // ... (Phần TokenValidationParameters giữ nguyên) ...
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // ... giữ nguyên code cũ của bạn
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
    };

    // 👇 THÊM ĐOẠN NÀY ĐỂ CUSTOM TRẢ VỀ JSON CHO 401 & 403
    options.Events = new JwtBearerEvents
    {
        // 1. Xử lý khi chưa đăng nhập (401 Unauthorized)
        OnChallenge = context =>
        {
            // Bỏ qua behavior mặc định (trả về header rỗng)
            context.HandleResponse();

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            var result = Result.Failure("Bạn chưa đăng nhập hoặc Token không hợp lệ.");
            var json = JsonSerializer.Serialize(result);

            return context.Response.WriteAsync(json);
        },

        // 2. Xử lý khi đăng nhập rồi nhưng không đủ quyền (403 Forbidden)
        OnForbidden = context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";

            var result = Result.Failure("Bạn không có quyền truy cập tài nguyên này (Role không đủ).");
            var json = JsonSerializer.Serialize(result);

            return context.Response.WriteAsync(json);
        }
    };
});
            builder.Services.AddAuthorization();

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
                // Lấy tên file XML theo tên Assembly (Project)
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                // 👇 THÊM ĐOẠN IF NÀY VÀO
                if (File.Exists(xmlPath))
                {
                    option.IncludeXmlComments(xmlPath);
                }
            });

            
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer(); // ❗ bắt buộc
            builder.Services.AddSwaggerGen();
            var app = builder.Build();
            //auto migration section
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    // Lấy service ra và chạy
                    var migrationService = services.GetRequiredService<IMigrationService>();


                    await migrationService.MigrateAsync();

                    Console.WriteLine("Database Migration & Seeding Completed!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Migration Failed: {ex.Message}");
                }
            }
            

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}