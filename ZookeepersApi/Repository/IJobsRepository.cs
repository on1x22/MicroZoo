using MicroZoo.Infrastructure.Generals;
using MicroZoo.Infrastructure.Models.Jobs;
using MicroZoo.Infrastructure.Models.Jobs.Dto;

namespace MicroZoo.ZookeepersApi.Repository
{
    public interface IJobsRepository
    {
        Task<List<Job>> GetAllJobsOfZookeeperAsync(int zookeeperId);

        Task<List<Job>> GetCurrentJobsOfZookeeperAsync(int zookeeperId);

        Task<List<Job>> GetAllJobsForDateTimeRangeAsync(DateTimeRange dateTimeRange, 
            OrderingOptions orderingOptions, PageOptions pageOptions);

        Task<List<Job>> GetZookeeperJobsForDateTimeRangeAsync(int zookeeperId,
            DateTimeRange dateTimeRange, OrderingOptions orderingOptions, PageOptions pageOptions);

        Task<Job> GetJobAsync(int jobId);

        Task<Job> AddJobAsync(JobDto jobDto);

        Task<Job> UpdateJobAsync(int jobId, JobWithoutStartTimeDto jobDto);

        Task<Job> FinishJobAsync(int jobId);
    }
}
