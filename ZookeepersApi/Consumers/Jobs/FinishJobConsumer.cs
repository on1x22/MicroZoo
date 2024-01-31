using MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.ZookeepersApi.Services;

namespace MicroZoo.ZookeepersApi.Consumers.Jobs
{
    public class FinishJobConsumer : IConsumer<FinishJobRequest>
    {
        private readonly IJobsRequestReceivingService _service;

        public FinishJobConsumer(IJobsRequestReceivingService servise)
        {
            _service = servise;
        }

        public async Task Consume(ConsumeContext<FinishJobRequest> context)
        {
            var response = await _service.FinishJobAsync(context.Message.JobId);

            if (response.Jobs == null && response.ErrorMessage == null)
                response.ErrorMessage = "Unknown error";

            response.OperationId = context.Message.OperationId;

            await context.RespondAsync(response);
        }
    }
}
