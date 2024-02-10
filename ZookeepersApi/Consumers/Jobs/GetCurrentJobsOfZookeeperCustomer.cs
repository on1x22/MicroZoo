using MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.ZookeepersApi.Services;

namespace MicroZoo.ZookeepersApi.Consumers.Jobs
{
    public class GetCurrentJobsOfZookeeperCustomer : IConsumer<GetCurrentJobsOfZookeeperRequest>
    {
        private readonly IJobsRequestReceivingService _service;

        public GetCurrentJobsOfZookeeperCustomer(IJobsRequestReceivingService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<GetCurrentJobsOfZookeeperRequest> context)
        {
            var response = await _service.GetCurrentJobsOfZookeeperAsync(context.Message.ZookeeperId);

            if (response.Jobs == null)
                response.ErrorMessage = "Unknown error";

            response.OperationId = context.Message.OperationId;

            await context.RespondAsync(response);
        }
    }
}
