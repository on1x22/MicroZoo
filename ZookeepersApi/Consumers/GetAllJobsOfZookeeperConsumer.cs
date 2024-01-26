﻿using MassTransit;
using MassTransit.JobService;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.ZookeepersApi.Services;

namespace ZookeepersApi.Consumers
{
    public class GetAllJobsOfZookeeperConsumer : IConsumer<GetAllJobsOfZookeeperRequest>
    {
        private readonly IJobsService _service;

        public GetAllJobsOfZookeeperConsumer(IJobsService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<GetAllJobsOfZookeeperRequest> context)
        {
            var response = await _service.GetAllJobsOfZookeeperAsync(context.Message.ZookeeperId);

            if (response.Jobs == null)
                response.ErrorMessage = "Unknown error";

            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetJobsResponse>(response);
        }
    }
}