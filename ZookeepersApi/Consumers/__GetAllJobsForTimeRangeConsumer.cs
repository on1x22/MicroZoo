﻿using MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.ZookeepersApi.Services;

namespace MicroZoo.ZookeepersApi.Consumers
{
    [Obsolete("Very bad solution. Please use GetJobsForTimeRangeConsumer", true)]
    public class __GetAllJobsForTimeRangeConsumer : IConsumer<__GetAllJobsForTimeRangeRequest>
    {
        private readonly IJobsService _service;

        public __GetAllJobsForTimeRangeConsumer(IJobsService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<__GetAllJobsForTimeRangeRequest> context)
        {
            /*var response = await _service.GetCurrent(context.Message.ZookeeperId);

            if (response.Jobs == null)
                response.ErrorMessage = "Unknown error";

            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetJobsResponse>(response);*/
        }
    }
}
