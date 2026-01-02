using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotApplication
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // 1. Đăng ký AutoMapper (quét toàn bộ assembly này)
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);

            // 2. Đăng ký MediatR (Thay thế cho việc đăng ký từng Service thủ công)
            // Nó sẽ tự tìm tất cả các file Handler để đăng ký
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            return services;
        }
    }
}
