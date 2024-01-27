using MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.ZookeepersApi.Services;

namespace MicroZoo.ZookeepersApi.Consumers
{
    public class GetJobsForTimeRangeConsumer : 
        IConsumer<GetJobsForTimeRangeRequest>
    {
        //private readonly IJobsService _service_old;
        private readonly IJobsRequestReceivingService _service;

        public GetJobsForTimeRangeConsumer(/*IJobsService service_old*/ IJobsRequestReceivingService servise)
        {
            //_service_old = service_old;
            _service = servise;
        }

        public async Task Consume(ConsumeContext<GetJobsForTimeRangeRequest> context)
        {   
            var response = await _service.GetJobsForTimeRangeAsync(context.Message.ZookeeperId,
                context.Message.StartDateTime, context.Message.FinishDateTime);

            if (response.Jobs == null)
                response.ErrorMessage = "Unknown error";

            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetJobsResponse>(response);
        }
    }
}
