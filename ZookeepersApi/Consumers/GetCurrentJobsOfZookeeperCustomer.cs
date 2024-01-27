using MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.ZookeepersApi.Services;

namespace MicroZoo.ZookeepersApi.Consumers
{
    public class GetCurrentJobsOfZookeeperCustomer : IConsumer<GetCurrentJobsOfZookeeperRequest>
    {
        //private readonly IJobsService _service;
        private readonly IJobsRequestReceivingService _service;

        public GetCurrentJobsOfZookeeperCustomer(/*IJobsService service_old*/ IJobsRequestReceivingService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<GetCurrentJobsOfZookeeperRequest> context)
        {
            //var response = await _service_old.GetCurrentJobsOfZookeeperAsync(context.Message.ZookeeperId);
            var response = await _service.GetCurrentJobsOfZookeeperAsync(context.Message.ZookeeperId);

            if (response.Jobs == null)
                response.ErrorMessage = "Unknown error";

            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetJobsResponse>(response);
        }
    }
}
