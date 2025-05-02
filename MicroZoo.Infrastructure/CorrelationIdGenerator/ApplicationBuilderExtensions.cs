using Microsoft.AspNetCore.Builder;

namespace MicroZoo.Infrastructure.CorrelationIdGenerator
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCorrelationIdMiddleware(this IApplicationBuilder applicationBuilder) =>
            applicationBuilder.UseMiddleware<CorrelationIdMiddleware>();        
    }
}
