﻿using MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.ZookeepersApi.Services;

namespace MicroZoo.ZookeepersApi.Consumers.Jobs
{
    public class GetJobsForTimeRangeConsumer :
        IConsumer<GetJobsForDateTimeRangeRequest>
    {
        private readonly IJobsRequestReceivingService _service;

        public GetJobsForTimeRangeConsumer(IJobsRequestReceivingService servise)
        {
            _service = servise;
        }

        public async Task Consume(ConsumeContext<GetJobsForDateTimeRangeRequest> context)
        {
            var response = await _service.GetJobsForDateTimeRangeAsync(context.Message.ZookeeperId,
                context.Message.DateTimeRange, context.Message.OrderingOptions,
                context.Message.PageOptions);

            if (response.Jobs == null)
                response.ErrorMessage = "Unknown error";

            response.OperationId = context.Message.OperationId;

            await context.RespondAsync(response);
        }
    }
}
