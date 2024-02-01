using Microsoft.Extensions.Logging;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Jobs;
using MicroZoo.ZookeepersApi.Repository;
using MicroZoo.ZookeepersApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroZoo.ZookeepersApi.Tests.UnitTests
{
    public class JobsServiceTests
    {
        [Fact]
        public async void GetAllJobsOfZookeeperAsync_should_return_all_jobs_of_zookeeper()
        {
            int id = new Fixture().Create<int>();
            List<Job> jobs = new List<Job>()
            {
                new Fixture().Build<Job>().With(j => j.ZookeeperId == id).Create(),
                new Fixture().Build<Job>().With(j => j.ZookeeperId == id).Create(),
                //new Fixture().Build<Job>().With(j => j.ZookeeperId == 2).Create()
            };

            var getJobsResponse = new GetJobsResponse()
            {
                Jobs = jobs
            };

            var mockRepo = new Mock<IJobsRepository>();
            mockRepo.Setup(j => j.GetAllJobsOfZookeeperAsync(It.IsAny<int>())).ReturnsAsync(jobs);
            var mockLogger = new Mock<ILogger<JobsService>>();

            var sut = new JobsService(mockRepo.Object, mockLogger.Object);

            var result = await sut.GetAllJobsOfZookeeperAsync(id);

            Assert.NotNull(result.Jobs);
        }
    }
}
