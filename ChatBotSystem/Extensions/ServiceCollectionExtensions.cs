using ChatBotInterfacture.Repositories;
using Domain.Interface.Repository;

namespace ChatBotSystem.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // Đăng ký GenericRepository
            services.AddScoped<ICourseRepository, CourseRepository>();
            return services;
        }
    }
}
