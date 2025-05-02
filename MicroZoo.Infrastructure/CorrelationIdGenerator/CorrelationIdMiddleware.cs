using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace MicroZoo.Infrastructure.CorrelationIdGenerator
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CorrelationIdMiddleware> _logger;
        private const string _correlationIdHeader = "X-Correlation-Id";

        public CorrelationIdMiddleware(RequestDelegate next,
            ILogger<CorrelationIdMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, ICorrelationIdGenerator correlationIdGenerator)
        {
            var correlationId = GetCorrelationId(context, correlationIdGenerator);            
            AddCorrelationIdHeaderToRequest(context, correlationId);

            _logger.LogInformation($"CorrelationIdMiddleware. Request with " +
                $"CorrelationId {correlationId} received");

            await _next(context);

            _logger.LogInformation($"CorrelationIdMiddleware. Request with " +
                $"CorrelationId {correlationId} processed");
        }

        private StringValues GetCorrelationId(HttpContext context,
            ICorrelationIdGenerator correlationIdGenerator)
        {
            if (context.Request.Headers.TryGetValue(_correlationIdHeader, out var correlationId))
            {
                correlationIdGenerator.SetCorrelationId(correlationId!);
                
                return correlationId;
            }
            else
            {
                var newCorrelationId = correlationIdGenerator.GetCorrelationId();
                
                return newCorrelationId;
            }
        }

        private void AddCorrelationIdHeaderToRequest(HttpContext context,
            StringValues correlationId)
        {
            if (context.Request.Headers[_correlationIdHeader].FirstOrDefault() == null)
                context.Request.Headers.Add(_correlationIdHeader, correlationId.ToString());
        }

    }
}
