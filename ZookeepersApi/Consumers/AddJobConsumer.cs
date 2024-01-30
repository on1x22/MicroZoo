using MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.ZookeepersApi.Services;

namespace MicroZoo.ZookeepersApi.Consumers
{
    public class AddJobConsumer : IConsumer<AddJobRequest>
    {
        private readonly IJobsRequestReceivingService _service;

        public AddJobConsumer(IJobsRequestReceivingService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<AddJobRequest> context)
        {
            var response = await _service.AddJobAsync(context.Message.JobDto);
            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetJobsResponse>(response);
        }
    }
}
