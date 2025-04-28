using MassTransit;

namespace MicroZoo.ZookeepersApi
{
    public class CorrelationIdSendFilter<T> : IFilter<SendContext<T>> where T : class
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CorrelationIdSendFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Probe(ProbeContext context) { }

        public async Task Send(SendContext<T> context, IPipe<SendContext<T>> next)
        {
            var correlationId = _httpContextAccessor.HttpContext?.Request
                .Headers["X-Correlation-Id"].FirstOrDefault()
                ?? Guid.NewGuid().ToString();

            context.Headers.Set("X-Correlation-Id", correlationId);

            await next.Send(context);
        }
    }
}
