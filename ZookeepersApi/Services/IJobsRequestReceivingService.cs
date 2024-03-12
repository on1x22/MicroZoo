using MicroZoo.Infrastructure.Generals;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Jobs.Dto;

namespace MicroZoo.ZookeepersApi.Services
{
    public interface IJobsRequestReceivingService
    {
        Task<GetJobsResponse> GetAllJobsOfZookeeperAsync(int zookeeperId, 
            PageOptions pageOptions, bool orderDesc);

        Task<GetJobsResponse> GetCurrentJobsOfZookeeperAsync(int zookeeperId);

        Task<GetJobsResponse> GetJobsForDateTimeRangeAsync(int zookeeperId, DateTime startDateTime,
            DateTime finishDateTime);

        Task<GetJobsResponse> AddJobAsync(JobDto jobDto);

        Task<GetJobsResponse> UpdateJobAsync(int jobId, JobWithoutStartTimeDto jobDto);

        Task<GetJobsResponse> FinishJobAsync(int jobId);
    }
}
