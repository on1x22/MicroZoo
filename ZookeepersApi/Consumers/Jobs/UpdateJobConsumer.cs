using MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.ZookeepersApi.Services;

namespace MicroZoo.ZookeepersApi.Consumers.Jobs
{
    public class UpdateJobConsumer : IConsumer<UpdateJobRequest>
    {
        private readonly IJobsRequestReceivingService _service;

        public UpdateJobConsumer(IJobsRequestReceivingService servise)
        {
            _service = servise;
        }

        public async Task Consume(ConsumeContext<UpdateJobRequest> context)
        {
            var response = await _service.UpdateJobAsync(context.Message.JobId,
                context.Message.JobDto, context.Message.AccessToken);

            if (response.Jobs == null && response.ErrorMessage == null)
                response.ErrorMessage = "Unknown error";

            response.OperationId = context.Message.OperationId;

            await context.RespondAsync(response);
        }
    }
}
