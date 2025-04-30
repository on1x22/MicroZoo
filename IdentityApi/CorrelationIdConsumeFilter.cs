using MassTransit;

namespace MicroZoo.IdentityApi
{
    public class CorrelationIdConsumeFilter<T> : IFilter<ConsumeContext<T>> where T : class
    {
        private readonly ILogger<CorrelationIdConsumeFilter<T>> _logger;

        public CorrelationIdConsumeFilter(ILogger<CorrelationIdConsumeFilter<T>> logger)
        {
            _logger = logger;
        }

        public void Probe(ProbeContext context) { }

        public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
        {
            var correlationId = context.Headers.Get<string>("X-Correlation-Id")
                ?? Guid.NewGuid().ToString();

            Serilog.Context.LogContext.PushProperty("CorrelationId", correlationId);

            _logger.LogInformation($"MassTransit CorrelationIdConsumeFilter<T> with " +
                $"correlationId {correlationId} started");

            await next.Send(context);

            _logger.LogInformation($"MassTransit CorrelationIdConsumeFilter<T> with " +
                $"correlationId {correlationId} finished");
        }
    }
}
