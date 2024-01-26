﻿using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Jobs;
using MicroZoo.ZookeepersApi.Repository;

namespace MicroZoo.ZookeepersApi.Services
{
    public class JobsService : IJobsService
    {
        private readonly IJobsRepository _repository;
        private readonly ILogger _logger;

        public JobsService(IJobsRepository repository, ILogger<JobsService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<GetJobsResponse> GetAllJobsOfZookeeperAsync(int zookeeperId) =>    
            new GetJobsResponse()
            {
                Jobs = await _repository.GetAllJobsOfZookeeperAsync(zookeeperId)
            };

        public async Task<GetJobsResponse> GetCurrentJobsOfZookeeperAsync(int zookeeperId) =>
            new GetJobsResponse()
            {
                Jobs = await _repository.GetCurrentJobsOfZookeeperAsync(zookeeperId)
            };

        public async Task<GetJobsResponse> GetJobsForTimeRangeAsync(int zookeeperId, DateTime startDateTime, DateTime finishDateTime)
        {
            var response = new GetJobsResponse();

            if(zookeeperId == 0)
                response.Jobs = await _repository.GetAllJobsForTimeRangeAsync(startDateTime, 
                    finishDateTime);
            
            if(zookeeperId > 0)
                response.Jobs = await _repository.GetZookeeperJobsForTimeRangeAsync(zookeeperId, 
                    startDateTime, finishDateTime);
            
            return response;
        }
    }
}
