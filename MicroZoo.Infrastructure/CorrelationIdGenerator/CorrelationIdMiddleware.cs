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
            //AddCorrelationIdHeaderToResponse(context, correlationId);

            await _next(context);
        }

        private StringValues GetCorrelationId(HttpContext context,
            ICorrelationIdGenerator correlationIdGenerator)
        {
            if (context.Request.Headers.TryGetValue(_correlationIdHeader, out var correlationId))
            {
                correlationIdGenerator.SetCorrelationId(correlationId!);
                _logger.LogInformation($"Request with CorrelationId {correlationId} received");
                return correlationId;
            }
            else
            {
                var newCorrelationId = correlationIdGenerator.GetCorrelationId();
                _logger.LogInformation($"The request had no CorrelationId, so it was assigned " +
                    $"automatically ({newCorrelationId})");
                return newCorrelationId;
            }
        }

        /*private void AddCorrelationIdHeaderToResponse(HttpContext context,
            StringValues correlationId)
        {
            _logger.LogInformation($"The response is assigned an CorrelationId {correlationId}");
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Add(_correlationIdHeader,
                        new[] { correlationId.ToString() });

                return Task.CompletedTask;
            });
        }*/
            
    }
}
