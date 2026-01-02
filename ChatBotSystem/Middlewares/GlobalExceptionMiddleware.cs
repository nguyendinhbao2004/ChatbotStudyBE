using Domain.Common;
using Domain.Exception;
using Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace ChatBotSystem.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                await _next(context);
                return;
            }
            try
            {
                // Cho request đi tiếp vào Controller
                await _next(context);
            }catch(Exception ex)
            {
                // Nếu có lỗi ném ra, bắt lấy và xử lý
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // Mặc định là lỗi 500 (Lỗi hệ thống)
            var response = Result.Failure("An internal error occurred.");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            switch (exception)
            {
                // 1. Lỗi Nghiệp vụ -> Trả về 400 (Bad Request)
                case DomainException domainException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response = Result.Failure(domainException.Message);
                    break;
               // 2. Lỗi Không tìm thấy -> Trả về 404 (Not Found)
                case EntityNotFoundException notFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    response = Result.Failure(notFoundException.Message);
                    break;

                // 3. Lỗi xác thực dữ liệu (Nếu dùng FluentValidation sau này)
                // case ValidationException validationEx:
                //     context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                //     response = Result.Failure(validationEx.Errors...);
                //     break;

                // 4. Lỗi hệ thống (Crash, NullRef...) -> Log lại để Dev sửa
                default:
                    _logger.LogError(exception, "System Crash: {Mesage}", exception.Message);
                    break;
            }
            // Chuyển object Result thành JSON để trả về Client
            var result = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase //format camelCase cho JS dễ đọc
            });
            return context.Response.WriteAsync(result);
        }
    }
}
