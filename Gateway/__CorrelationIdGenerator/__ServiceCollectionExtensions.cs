namespace MicroZoo.Gateway.CorrelationIdGenerator
{
    public static class __ServiceCollectionExtensions
    {
        public static IServiceCollection AddCorrelationIdGenerator(this IServiceCollection services)
        {
            services.AddScoped<__ICorrelationIdGenerator, __CorrelationIdGenerator>();

            return services;
        }
    }
}
