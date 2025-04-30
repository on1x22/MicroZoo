using MassTransit;

namespace MicroZoo.ZookeepersApi
{
    public class CorrelationIdSendFilter<T> : IFilter<SendContext<T>> where T : class
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CorrelationIdSendFilter<T>> _logger;

        public CorrelationIdSendFilter(IHttpContextAccessor httpContextAccessor,
            ILogger<CorrelationIdSendFilter<T>> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public void Probe(ProbeContext context) { }

        public async Task Send(SendContext<T> context, IPipe<SendContext<T>> next)
        {
            var correlationId = _httpContextAccessor.HttpContext?.Request
                .Headers["X-Correlation-Id"].FirstOrDefault()
                ?? Guid.NewGuid().ToString();

            context.Headers.Set("X-Correlation-Id", correlationId);

            _logger.LogInformation($"MassTransit CorrelationIdSendFilter<T> with " +
                $"correlationId {correlationId} started");

            await next.Send(context);

            _logger.LogInformation($"MassTransit CorrelationIdSendFilter<T> with " +
                $"correlationId {correlationId} finished");
        }
    }
}
