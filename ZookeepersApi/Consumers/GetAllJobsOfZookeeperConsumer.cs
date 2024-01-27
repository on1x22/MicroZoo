using MassTransit;
using MassTransit.JobService;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.ZookeepersApi.Services;

namespace ZookeepersApi.Consumers
{
    public class GetAllJobsOfZookeeperConsumer : IConsumer<GetAllJobsOfZookeeperRequest>
    {
        //private readonly IJobsService _service_old;
        private readonly IJobsRequestReceivingService _service;

        public GetAllJobsOfZookeeperConsumer(/*IJobsService service_old*/ IJobsRequestReceivingService servise)
        {
            //_service_old = service_old;
            _service = servise;
        }

        public async Task Consume(ConsumeContext<GetAllJobsOfZookeeperRequest> context)
        {
            //var response = await _service_old.GetAllJobsOfZookeeperAsync(context.Message.ZookeeperId);
            var response = await _service.GetAllJobsOfZookeeperAsync(context.Message.ZookeeperId);

            if (response.Jobs == null)
                response.ErrorMessage = "Unknown error";

            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetJobsResponse>(response);
        }
    }
}
