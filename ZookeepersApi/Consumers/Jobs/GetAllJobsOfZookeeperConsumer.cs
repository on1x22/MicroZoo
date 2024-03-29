﻿using MassTransit;
using MassTransit.JobService;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.ZookeepersApi.Services;

namespace MicroZoo.ZookeepersApi.Consumers.Jobs
{
    public class GetAllJobsOfZookeeperConsumer : IConsumer<GetAllJobsOfZookeeperRequest>
    {
        private readonly IJobsRequestReceivingService _service;

        public GetAllJobsOfZookeeperConsumer(IJobsRequestReceivingService servise)
        {
            _service = servise;
        }

        public async Task Consume(ConsumeContext<GetAllJobsOfZookeeperRequest> context)
        {
            var response = await _service.GetAllJobsOfZookeeperAsync(context.Message.ZookeeperId);

            if (response.Jobs == null)
                response.ErrorMessage = "Unknown error";

            response.OperationId = context.Message.OperationId;

            await context.RespondAsync(response);
        }
    }
}
