﻿using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Jobs;
using MicroZoo.Infrastructure.Models.Jobs.Dto;
using MicroZoo.ZookeepersApi.Models;

namespace MicroZoo.ZookeepersApi.Services
{
    public interface IJobsService
    {
        Task<GetJobsResponse> GetAllJobsOfZookeeperAsync(int zookeeperId);

        Task<GetJobsResponse> GetCurrentJobsOfZookeeperAsync(int zookeeperId);

        Task<GetJobsResponse> GetJobsForTimeRangeAsync(int zookeeperId, DateTime startDateTime,
            DateTime finishDateTime);

        Task<GetJobResponse> AddJobAsync(JobDto jobDto);

        Task<GetJobResponse> UpdateJobAsync(int jobId, JobWithoutStartTimeDto jobDto);

        Task<GetJobResponse> FinishJobAsync(int jobId);
    }
}
