using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MicroZoo.Infrastructure.CorrelationIdGenerator;
using Serilog;


namespace MicroZoo.Infrastructure.MassTransit
{
    public class CorrelationIdHandler
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
