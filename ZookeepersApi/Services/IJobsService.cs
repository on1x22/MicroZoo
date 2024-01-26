using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Jobs;
using MicroZoo.ZookeepersApi.Models;

namespace MicroZoo.ZookeepersApi.Services
{
    public interface IJobsService
    {
        Task<GetJobsResponse> GetAllJobsOfZookeeperAsync(int zookeeperId);

        Task<GetJobsResponse> GetCurrentJobsOfZookeeperAsync(int zookeeperId);

        Task<GetJobsResponse> GetJobsForTimeRangeAsync(int zookeeperId, DateTime startDateTime,
            DateTime finishDateTime);
    }
}
