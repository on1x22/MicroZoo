using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
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
    }
}
