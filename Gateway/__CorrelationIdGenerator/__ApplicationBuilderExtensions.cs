namespace MicroZoo.Gateway.CorrelationIdGenerator
{
    public static class __ApplicationBuilderExtensions
    {
        public static IApplicationBuilder __UseCorrelationIdMiddleware(this IApplicationBuilder applicationBuilder) =>
            applicationBuilder.UseMiddleware<__CorrelationIdMiddleware>();        
    }
}
