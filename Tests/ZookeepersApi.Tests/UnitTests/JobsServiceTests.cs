using Microsoft.Extensions.Logging;
using MicroZoo.Infrastructure.Generals;
using MicroZoo.Infrastructure.Models.Jobs;
using MicroZoo.Infrastructure.Models.Jobs.Dto;
using MicroZoo.ZookeepersApi.Repository;
using MicroZoo.ZookeepersApi.Services;

namespace MicroZoo.ZookeepersApi.Tests.UnitTests
{
    public class JobsServiceTests
    {
        private Mock<IJobsRepository> _mockRepo;
        private Mock<ILogger<JobsService>> _mockLogger;

        public JobsServiceTests()
        {
            _mockRepo = new Mock<IJobsRepository>();
            _mockLogger = new Mock<ILogger<JobsService>>();
        }

        [Fact]
        public async void GetAllJobsOfZookeeperAsync_should_return_all_jobs_of_zookeeper()
        {
            int zookeeperId = new Fixture().Create<int>();
            List<Job> jobs = JobsFactory.GetListOfAllJobs(zookeeperId);           
            var expectedJobs = new List<Job>(jobs);

            _mockRepo.Setup(j => j.GetAllJobsOfZookeeperAsync(It.IsAny<int>())).ReturnsAsync(jobs);

            var jobService = new JobsService(_mockRepo.Object, _mockLogger.Object);

            var result = await jobService.GetAllJobsOfZookeeperAsync(zookeeperId);

            Assert.Equal(expectedJobs, result.Jobs);
        }

        [Fact]
        public async void GetAllJobsOfZookeeperAsync_should_return_zero_entities()
        {
            int zookeeperId = new Fixture().Create<int>();
            List<Job> jobs = new List<Job>();

            _mockRepo.Setup(j => j.GetAllJobsOfZookeeperAsync(It.IsAny<int>())).ReturnsAsync(jobs);

            var jobService = new JobsService(_mockRepo.Object, _mockLogger.Object);

            var result = await jobService.GetAllJobsOfZookeeperAsync(zookeeperId);

            Assert.NotNull(result.Jobs);
            Assert.Empty(result.Jobs);
        }

        [Fact]
        public async void GetCurrentJobsOfZookeeperAsync_should_return_all_current_jobs_of_zookeeper()
        {
            int zookeeperId = new Fixture().Create<int>();

            List<Job> allJobs = JobsFactory.GetListOfAllJobs(zookeeperId); 
            List<Job> jobsOfSelectedZookeeper = JobsFactory.GetJobsOfSelectedZookeeper(allJobs, zookeeperId);

            _mockRepo.Setup(j => j.GetCurrentJobsOfZookeeperAsync(It.IsAny<int>()))
                .ReturnsAsync(jobsOfSelectedZookeeper);


            var jobService = new JobsService(_mockRepo.Object, _mockLogger.Object);

            var result = await jobService.GetCurrentJobsOfZookeeperAsync(zookeeperId);

            Assert.Equal(jobsOfSelectedZookeeper.Count, result.Jobs.Count);
        }

        [Fact]
        public async void GetCurrentJobsOfZookeeperAsync_should_return_zero_current_jobs_of_zookeeper()
        {
            int zookeeperId = new Fixture().Create<int>();
            List<Job> jobs = new List<Job>();

            _mockRepo.Setup(j => j.GetCurrentJobsOfZookeeperAsync(It.IsAny<int>())).ReturnsAsync(jobs);

            var jobService = new JobsService(_mockRepo.Object, _mockLogger.Object);

            var result = await jobService.GetCurrentJobsOfZookeeperAsync(zookeeperId);

            Assert.NotNull(result.Jobs);
            Assert.Empty(result.Jobs);
        }

        [Theory]
        [MemberData(nameof(DateTimeRangeOrderingOptionsPageOptions))]
        public async void GetJobsForTimeRangeAsync_should_return_all_jobs_for_time_range(
            DateTimeRange dateTimeRange, OrderingOptions orderingOptions, PageOptions pageOptions)
        {
            int zookeeperId = 0;

            int randomZookeeperId = new Fixture().Create<int>();
            List<Job> allJobs = JobsFactory.GetListOfAllJobs(randomZookeeperId);
            List<Job> jobsOfRandomZookeeper = JobsFactory.GetJobsOfSelectedZookeeper(allJobs, randomZookeeperId);

            _mockRepo.Setup(j => j.GetAllJobsForDateTimeRangeAsync(It.IsAny<DateTimeRange>(),
                It.IsAny<OrderingOptions>(), It.IsAny<PageOptions>())).ReturnsAsync(allJobs);
            _mockRepo.Setup(j => j.GetZookeeperJobsForDateTimeRangeAsync(randomZookeeperId,
                It.IsAny<DateTimeRange>(), It.IsAny<OrderingOptions>(), It.IsAny<PageOptions>()))
                .ReturnsAsync(jobsOfRandomZookeeper);

            var jobService = new JobsService(_mockRepo.Object, _mockLogger.Object);

            var result = await jobService.GetJobsForDateTimeRangeAsync(zookeeperId, dateTimeRange, 
                orderingOptions, pageOptions);

            Assert.NotEqual(jobsOfRandomZookeeper, result.Jobs);
            Assert.Equal(allJobs, result.Jobs);
        }

        [Theory]
        [MemberData(nameof(DateTimeRangeOrderingOptionsPageOptions))]
        public async void GetJobsForTimeRangeAsync_should_return_jobs_of_selected_zookeeper_for_time_range(
            DateTimeRange dateTimeRange, OrderingOptions orderingOptions, PageOptions pageOptions)
        {
            int zookeeperId = new Fixture().Create<int>();

            List<Job> allJobs = JobsFactory.GetListOfAllJobs(zookeeperId);

            List<Job> jobsOfSelectedZookeeper = JobsFactory.GetJobsOfSelectedZookeeper(allJobs, zookeeperId);
            List<Job> jobsOfOtherZookeepers = allJobs
                .Where(j => j.ZookeeperId != zookeeperId).ToList();

            var expectedJobs = JobsFactory.GetJobsOfSelectedZookeeper(allJobs, zookeeperId);
         
            _mockRepo.Setup(j => j.GetAllJobsForDateTimeRangeAsync(It.IsAny<DateTimeRange>(),
                It.IsAny<OrderingOptions>(), It.IsAny<PageOptions>())).ReturnsAsync(allJobs);
            _mockRepo.Setup(j => j.GetZookeeperJobsForDateTimeRangeAsync(zookeeperId,
                It.IsAny<DateTimeRange>(), It.IsAny<OrderingOptions>(), It.IsAny<PageOptions>()))
                .ReturnsAsync(jobsOfSelectedZookeeper);

            var jobService = new JobsService(_mockRepo.Object, _mockLogger.Object);

            var result = await jobService.GetJobsForDateTimeRangeAsync(zookeeperId, dateTimeRange, 
                orderingOptions, pageOptions);

            Assert.NotEqual(jobsOfOtherZookeepers, result.Jobs);
            Assert.Equal(expectedJobs, result.Jobs);
        }
        
        [Fact]
        public async void AddJobAsync_should_return_job()
        {
            var jobDto = new Fixture().Build<JobDto>().Create();
            var job = new Fixture().Build<Job>().With(j => j.ZookeeperId, jobDto.ZookeeperId)
                .With(j => j.Description, jobDto.Description)
                .With(j => j.StartTime, jobDto.StartTime)
                .Create();

            _mockRepo.Setup(j => j.AddJobAsync(It.IsAny<JobDto>())).ReturnsAsync(job);

            var jobService = new JobsService(_mockRepo.Object, _mockLogger.Object);

            var result = await jobService.AddJobAsync(jobDto);

            Assert.Null(result.ErrorMessage);
            Assert.Equal(job, result.Job);
        }

        [Fact]
        public async void AddJobAsync_should_return_error_message()
        {
            var expectedMessage = "Failed to create a new task. Please check the entered data";
            var jobDto = new Fixture().Build<JobDto>().Create();
            Job? job = null;

            _mockRepo.Setup(j => j.AddJobAsync(It.IsAny<JobDto>())).ReturnsAsync(job);

            var jobService = new JobsService(_mockRepo.Object, _mockLogger.Object);

            var result = await jobService.AddJobAsync(jobDto);

            Assert.Null(result.Job);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void UpdateJobAsync_should_return_error_message_task_with_id_not_exist()
        {
            int jobId = new Fixture().Create<int>();
            var expectedMessage = $"Task with id={jobId} not exist";
            Job? oldJob = null;
            var jobWithoutStartTimeDto = new Fixture().Create<JobWithoutStartTimeDto>();
           
            _mockRepo.Setup(j => j.GetJobAsync(It.IsAny<int>())).ReturnsAsync(oldJob);
            _mockRepo.Setup(j => j.UpdateJobAsync(It.IsAny<int>(), 
                It.IsAny<JobWithoutStartTimeDto>())).ReturnsAsync(oldJob);
            
            var jobService = new JobsService(_mockRepo.Object, _mockLogger.Object);

            var result = await jobService.UpdateJobAsync(jobId, jobWithoutStartTimeDto);

            Assert.Null(result.Job);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void UpdateJobAsync_should_return_error_message_task_is_completed()
        {
            int jobId = new Fixture().Create<int>();
            var expectedMessage = "Changing a completed task is not allowed";
            var jobWithoutStartTimeDto = new Fixture().Build<JobWithoutStartTimeDto>().Create();
            
            var oldJob = new Fixture().Build<Job>()
                .With(j => j.Id, jobId)
                .Create();

            _mockRepo.Setup(j => j.GetJobAsync(It.IsAny<int>())).ReturnsAsync(oldJob);
            _mockRepo.Setup(j => j.UpdateJobAsync(It.IsAny<int>(),
                It.IsAny<JobWithoutStartTimeDto>())).ReturnsAsync(oldJob);

            var jobService = new JobsService(_mockRepo.Object, _mockLogger.Object);

            var result = await jobService.UpdateJobAsync(jobId, jobWithoutStartTimeDto);

            Assert.Null(result.Job);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void UpdateJobAsync_should_return_error_message_error_during_update_job()
        {
            int jobId = new Fixture().Create<int>();
            var expectedMessage = "Failed to update task. Please check the entered data";
            var jobWithoutStartTimeDto = new Fixture().Build<JobWithoutStartTimeDto>().Create();
            DateTime? finishTime = null;

            var oldJob = new Fixture().Build<Job>()
                .With(j => j.Id, jobId)
                .With(j => j.FinishTime, finishTime)
                .Create();

            Job? updatedJob = null;

            _mockRepo.Setup(j => j.GetJobAsync(It.IsAny<int>())).ReturnsAsync(oldJob);
            _mockRepo.Setup(j => j.UpdateJobAsync(It.IsAny<int>(),
                It.IsAny<JobWithoutStartTimeDto>())).ReturnsAsync(updatedJob);

            var jobService = new JobsService(_mockRepo.Object, _mockLogger.Object);

            var result = await jobService.UpdateJobAsync(jobId, jobWithoutStartTimeDto);

            Assert.Null(result.Job);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void UpdateJobAsync_should_return_updated_job()
        {
            int jobId = new Fixture().Create<int>();
            var jobWithoutStartTimeDto = new Fixture().Build<JobWithoutStartTimeDto>().Create();
            DateTime? finishTime = null;

            var oldJob = new Fixture().Build<Job>()
                .With(j => j.Id, jobId)
                .With(j => j.FinishTime, finishTime)
                .Create();

            var updatedJob = new Fixture().Build<Job>()
                .With(j => j.Id, oldJob.Id)
                .With(j => j.ZookeeperId, jobWithoutStartTimeDto.ZookeeperId)
                .With(j => j.Description, jobWithoutStartTimeDto.Description)
                .With(j => j.StartTime, oldJob.StartTime)
                .With(j => j.FinishTime, oldJob.FinishTime)
                .Create();

            _mockRepo.Setup(j => j.GetJobAsync(It.IsAny<int>())).ReturnsAsync(oldJob);
            _mockRepo.Setup(j => j.UpdateJobAsync(It.IsAny<int>(),
                It.IsAny<JobWithoutStartTimeDto>())).ReturnsAsync(updatedJob);

            var jobService = new JobsService(_mockRepo.Object, _mockLogger.Object);

            var result = await jobService.UpdateJobAsync(jobId, jobWithoutStartTimeDto);

            Assert.Null(result.ErrorMessage);
            Assert.Equal(updatedJob, result.Job);
        }

        [Fact]
        public async void FinishJobAsync_should_return_error_message_task_with_id_not_exist()
        {
            int jobId = new Fixture().Create<int>();
            var expectedMessage = $"Task with id={jobId} not exist";
            Job? finishedJob = null;

            _mockRepo.Setup(j => j.GetJobAsync(It.IsAny<int>())).ReturnsAsync(finishedJob);
            _mockRepo.Setup(j => j.FinishJobAsync(It.IsAny<int>())).ReturnsAsync(finishedJob);

            var jobService = new JobsService(_mockRepo.Object, _mockLogger.Object);

            var result = await jobService.FinishJobAsync(jobId);

            Assert.Null(result.Job);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void FinishJobAsync_should_return_error_message_task_is_completed()
        {
            int jobId = new Fixture().Create<int>();
            var expectedMessage = $"Task wint id={jobId} already completed";

            var finishedJob = new Fixture().Build<Job>()
                .With(j => j.Id, jobId)
                .Create();

            _mockRepo.Setup(j => j.GetJobAsync(It.IsAny<int>())).ReturnsAsync(finishedJob);
            _mockRepo.Setup(j => j.FinishJobAsync(It.IsAny<int>())).ReturnsAsync(finishedJob);

            var jobService = new JobsService(_mockRepo.Object, _mockLogger.Object);

            var result = await jobService.FinishJobAsync(jobId);

            Assert.Null (result.Job);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void FinishJobAsync_should_return_error_message_error_during_finishing_job()
        {
            int jobId = new Fixture().Create<int>();
            var expectedMessage = "Failed to complete task. Please check the entered data";
            DateTime? finishTime = null;

            var jobBeforeFinish = new Fixture().Build<Job>()
                .With(j => j.Id, jobId)
                .With(j => j.FinishTime, finishTime)
                .Create();
            
            Job? jobAfterFinish = null;

            _mockRepo.Setup(j => j.GetJobAsync(It.IsAny<int>())).ReturnsAsync(jobBeforeFinish);
            _mockRepo.Setup(j => j.FinishJobAsync(It.IsAny<int>())).ReturnsAsync(jobAfterFinish);

            var jobService = new JobsService(_mockRepo.Object, _mockLogger.Object);

            var result = await jobService.FinishJobAsync(jobId);

            Assert.Null(result.Job);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void FinishJobAsync_should_return_finished_job()
        {
            int jobId = new Fixture().Create<int>();
            DateTime? finishTime = null;

            var jobBeforeFinish = new Fixture().Build<Job>()
                .With(j => j.Id, jobId)
                .With(j => j.FinishTime, finishTime)
                .Create();

            var jobAfterFinish = new Fixture().Build<Job>()
                .With(j => j.Id, jobBeforeFinish.Id)
                .With(j => j.Description, jobBeforeFinish.Description)
                .With(j => j.StartTime, jobBeforeFinish.StartTime)
                .Create();

            _mockRepo.Setup(j => j.GetJobAsync(It.IsAny<int>())).ReturnsAsync(jobBeforeFinish);
            _mockRepo.Setup(j => j.FinishJobAsync(It.IsAny<int>())).ReturnsAsync(jobAfterFinish);

            var jobService = new JobsService(_mockRepo.Object, _mockLogger.Object);

            var result = await jobService.FinishJobAsync(jobId);

            Assert.Equal(jobAfterFinish, result.Job);
        }


        public static IEnumerable<object[]> DateTimeRangeOrderingOptionsPageOptions
        {
            get
            {
                yield return new object[]
                {
                    new Fixture().Build<DateTimeRange>().Create(),
                    new Fixture().Build<OrderingOptions>().Create(),
                    new Fixture().Build<PageOptions>().Create()
                };
            }
        }
    }
}
