using MicroZoo.Infrastructure.Models.Jobs;

namespace MicroZoo.ZookeepersApi.Tests.UnitTests
{
    public class JobsFactory
    {   
        public static List<Job> GetListOfAllJobs(int zookeeperId)
        {
            return new List<Job>()
            {
                new Fixture().Build<Job>().Create(),
                new Fixture().Build<Job>().With(j => j.ZookeeperId, zookeeperId).Create(),
                new Fixture().Build<Job>().With(j => j.ZookeeperId, zookeeperId).Create(),
                new Fixture().Build<Job>().Create()
            };
        }

        public static List<Job> GetJobsOfSelectedZookeeper(List<Job> jobsList, int zookeeperId)
        {
            return jobsList.Where(j => j.ZookeeperId == zookeeperId).ToList();
        }
    }
}
