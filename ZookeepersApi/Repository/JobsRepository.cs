using Microsoft.EntityFrameworkCore;
using MicroZoo.Infrastructure.Models.Jobs;
using MicroZoo.ZookeepersApi.DBContext;

namespace MicroZoo.ZookeepersApi.Repository
{
    public class JobsRepository : IJobsRepository
    {
        private readonly ZookeeperDBContext _dBContext;
        
        public JobsRepository(ZookeeperDBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task<List<Job>> GetAllJobsOfZookeeperAsync(int zookeeperId) =>
            await _dBContext.Jobs.Where(j => j.ZookeeperId == zookeeperId)
                                        .OrderBy(j => j.StartTime).ToListAsync();

        public async Task<List<Job>> GetCurrentJobsOfZookeeperAsync(int zookeeperId) =>
            await _dBContext.Jobs.Where(j => j.ZookeeperId == zookeeperId && j.FinishTime == null)
                                 .OrderBy(j => j.StartTime).ToListAsync();

        private async Task SaveChangesAsync() =>
            await _dBContext.SaveChangesAsync();
    }
}
