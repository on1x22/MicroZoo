using MassTransit;
using MicroZoo.Infrastructure.CorrelationIdGenerator;


namespace MicroZoo.Infrastructure.MassTransit
{
    public class __CorrelationIdHandler
    {
        public static void SetCorrelationIdToLogContext(ConsumeContext context, 
            ICorrelationIdGenerator correlationIdGenerator)
        {
            var correlationId = context.Headers.Get<string>("X-Correlation-Id");
            if (correlationId != null)
                correlationIdGenerator.SetCorrelationId(correlationId);
            else
                correlationId = correlationIdGenerator.GetCorrelationId();
            Serilog.Context.LogContext.PushProperty("CorrelationId", correlationId);
        }
    }
}
