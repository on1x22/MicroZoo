using MicroZoo.Infrastructure.Models.Jobs;

namespace MicroZoo.ZookeepersApi.Repository
{
    public interface IJobsRepository
    {
        Task<List<Job>> GetAllJobsOfZookeeperAsync(int zookeeperId);

        Task<List<Job>> GetCurrentJobsOfZookeeperAsync(int zookeeperId);
    }
}
