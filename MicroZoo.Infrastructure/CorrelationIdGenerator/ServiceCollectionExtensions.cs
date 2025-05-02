using Microsoft.Extensions.DependencyInjection;

namespace MicroZoo.Infrastructure.CorrelationIdGenerator
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCorrelationIdGenerator(this IServiceCollection services)
        {
            services.AddScoped<ICorrelationIdGenerator, CorrelationIdGenerator>();

            return services;
        }
    }
}
