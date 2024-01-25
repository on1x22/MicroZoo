using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.ZookeepersApi.Models;

namespace MicroZoo.ZookeepersApi.Services
{
    public interface IJobsService
    {
        Task<GetJobsResponse> GetAllJobsOfZookeeperAsync(int zookeeperId);
    }
}
