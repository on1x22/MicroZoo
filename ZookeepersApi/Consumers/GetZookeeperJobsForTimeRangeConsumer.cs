using MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.ZookeepersApi.Services;

namespace MicroZoo.ZookeepersApi.Consumers
{
    public class GetZookeeperJobsForTimeRangeConsumer : 
        IConsumer<GetZookeeperJobsForTimeRangeRequest>
    {
        private readonly IJobsService _service;

        public GetZookeeperJobsForTimeRangeConsumer(IJobsService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<GetZookeeperJobsForTimeRangeRequest> context)
        {
            var response = await _service.GetCurrent(context.Message.ZookeeperId);

            if (response.Jobs == null)
                response.ErrorMessage = "Unknown error";

            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetJobsResponse>(response);
        }
    }
}
