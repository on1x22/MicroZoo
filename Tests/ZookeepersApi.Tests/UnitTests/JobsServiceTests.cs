using Microsoft.Extensions.Logging;
using MicroZoo.Infrastructure.Models.Jobs;
using MicroZoo.Infrastructure.Models.Jobs.Dto;
using MicroZoo.ZookeepersApi.Repository;
using MicroZoo.ZookeepersApi.Services;

namespace MicroZoo.ZookeepersApi.Tests.UnitTests
{
    public class JobsServiceTests
    {
        [Fact]
        public async void GetAllJobsOfZookeeperAsync_should_return_all_jobs_of_zookeeper()
        {
            int zookeeperId = new Fixture().Create<int>();
            List<Job> jobs = new List<Job>()
            {
                new Fixture().Build<Job>().With(j => j.ZookeeperId, zookeeperId).Create(),
                new Fixture().Build<Job>().With(j => j.ZookeeperId, zookeeperId).Create()
            };

            var mockRepo = new Mock<IJobsRepository>();
            mockRepo.Setup(j => j.GetAllJobsOfZookeeperAsync(It.IsAny<int>())).ReturnsAsync(jobs);
            var mockLogger = new Mock<ILogger<JobsService>>();

            var jobService = new JobsService(mockRepo.Object, mockLogger.Object);

            var result = await jobService.GetAllJobsOfZookeeperAsync(zookeeperId);

            Assert.Equal(jobs, result.Jobs);
        }

        [Fact]
        public async void GetAllJobsOfZookeeperAsync_should_return_zero_entities()
        {
            int zookeeperId = new Fixture().Create<int>();
            List<Job> jobs = new List<Job>();

            var mockRepo = new Mock<IJobsRepository>();
            mockRepo.Setup(j => j.GetAllJobsOfZookeeperAsync(It.IsAny<int>())).ReturnsAsync(jobs);
            var mockLogger = new Mock<ILogger<JobsService>>();

            var jobService = new JobsService(mockRepo.Object, mockLogger.Object);

            var result = await jobService.GetAllJobsOfZookeeperAsync(zookeeperId);

            Assert.NotNull(result.Jobs);
            Assert.Empty(result.Jobs);
        }

        [Fact]
        public async void GetCurrentJobsOfZookeeperAsync_should_return_all_current_jobs_of_zookeeper()
        {
            int zookeeperId = new Fixture().Create<int>();
            List<Job> jobs = new List<Job>()
            {
                new Fixture().Build<Job>().With(j => j.ZookeeperId, zookeeperId).Create(),
                new Fixture().Build<Job>().With(j => j.ZookeeperId, zookeeperId).Create()
            };

            var mockRepo = new Mock<IJobsRepository>();
            mockRepo.Setup(j => j.GetCurrentJobsOfZookeeperAsync(It.IsAny<int>())).ReturnsAsync(jobs);
            var mockLogger = new Mock<ILogger<JobsService>>();

            var jobService = new JobsService(mockRepo.Object, mockLogger.Object);

            var result = await jobService.GetCurrentJobsOfZookeeperAsync(zookeeperId);

            Assert.Equal(2, result.Jobs.Count);
        }

        [Fact]
        public async void GetCurrentJobsOfZookeeperAsync_should_return_zero_current_jobs_of_zookeeper()
        {
            int zookeeperId = new Fixture().Create<int>();
            List<Job> jobs = new List<Job>();

            var mockRepo = new Mock<IJobsRepository>();
            mockRepo.Setup(j => j.GetCurrentJobsOfZookeeperAsync(It.IsAny<int>())).ReturnsAsync(jobs);
            var mockLogger = new Mock<ILogger<JobsService>>();

            var jobService = new JobsService(mockRepo.Object, mockLogger.Object);

            var result = await jobService.GetCurrentJobsOfZookeeperAsync(zookeeperId);

            Assert.NotNull(result.Jobs);
            Assert.Empty(result.Jobs);
        }

        [Fact]
        public async void GetJobsForTimeRangeAsync_should_return_all_jobs_for_time_range()
        {
            int zookeeperId = 0;

            int randomZookeeperId = new Fixture().Create<int>();
            List<Job> allJobs = new List<Job>()
            {
                new Fixture().Build<Job>().Create(),
                new Fixture().Build<Job>().With(j => j.ZookeeperId, randomZookeeperId).Create(),
                new Fixture().Build<Job>().With(j => j.ZookeeperId, randomZookeeperId).Create(),
                new Fixture().Build<Job>().Create()
            };

            List<Job> jobsOfRandomZookeeper = allJobs
                .Where(j => j.ZookeeperId == randomZookeeperId).ToList();
            DateTime startDateTime = DateTime.Now.AddDays(-1);
            DateTime endDateTime = DateTime.Now.AddDays(1);


            var mockRepo = new Mock<IJobsRepository>();
            mockRepo.Setup(j => j.GetAllJobsForTimeRangeAsync(It.IsAny<DateTime>(),
                It.IsAny<DateTime>())).ReturnsAsync(allJobs);
            mockRepo.Setup(j => j.GetZookeeperJobsForTimeRangeAsync(randomZookeeperId,
                It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(jobsOfRandomZookeeper);
            var mockLogger = new Mock<ILogger<JobsService>>();

            var jobService = new JobsService(mockRepo.Object, mockLogger.Object);

            var result = await jobService.GetJobsForTimeRangeAsync(zookeeperId, startDateTime, 
                endDateTime);

            Assert.Equal(allJobs, result.Jobs);
        }

        [Fact]
        public async void GetJobsForTimeRangeAsync_should_return_jobs_of_selected_zookeeper_for_time_range()
        {
            int zookeeperId = new Fixture().Create<int>();

            List<Job> allJobs = new List<Job>()
            {
                new Fixture().Build<Job>().Create(),
                new Fixture().Build<Job>().With(j => j.ZookeeperId, zookeeperId).Create(),
                new Fixture().Build<Job>().With(j => j.ZookeeperId, zookeeperId).Create(),
                new Fixture().Build<Job>().Create()
            };

            List<Job> jobsOfSelectedZookeeper = allJobs
                .Where(j => j.ZookeeperId == zookeeperId).ToList();
            DateTime startDateTime = DateTime.Now.AddDays(-1);
            DateTime endDateTime = DateTime.Now.AddDays(1);


            var mockRepo = new Mock<IJobsRepository>();
            mockRepo.Setup(j => j.GetAllJobsForTimeRangeAsync(It.IsAny<DateTime>(),
                It.IsAny<DateTime>())).ReturnsAsync(allJobs);
            mockRepo.Setup(j => j.GetZookeeperJobsForTimeRangeAsync(zookeeperId,
                It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(jobsOfSelectedZookeeper);
            var mockLogger = new Mock<ILogger<JobsService>>();

            var jobService = new JobsService(mockRepo.Object, mockLogger.Object);

            var result = await jobService.GetJobsForTimeRangeAsync(zookeeperId, startDateTime,
                endDateTime);

            Assert.Equal(jobsOfSelectedZookeeper, result.Jobs);
        }
        
        [Fact]
        public async void AddJobAsync_should_return_job()
        {
            var jobDto = new Fixture().Build<JobDto>().Create();
            var job = new Fixture().Build<Job>().With(j => j.ZookeeperId, jobDto.ZookeeperId)
                .With(j => j.Description, jobDto.Description)
                .With(j => j.StartTime, jobDto.StartTime)
                .Create();

            var mockRepo = new Mock<IJobsRepository>();
            mockRepo.Setup(j => j.AddJobAsync(It.IsAny<JobDto>())).ReturnsAsync(job);
            var mockLogger = new Mock<ILogger<JobsService>>();

            var jobService = new JobsService(mockRepo.Object, mockLogger.Object);

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

            var mockRepo = new Mock<IJobsRepository>();
            mockRepo.Setup(j => j.AddJobAsync(It.IsAny<JobDto>())).ReturnsAsync(job);
            var mockLogger = new Mock<ILogger<JobsService>>();

            var jobService = new JobsService(mockRepo.Object, mockLogger.Object);

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
            var mockRepo = new Mock<IJobsRepository>();
            mockRepo.Setup(j => j.GetJobAsync(It.IsAny<int>())).ReturnsAsync(oldJob);
            mockRepo.Setup(j => j.UpdateJobAsync(It.IsAny<int>(), 
                It.IsAny<JobWithoutStartTimeDto>())).ReturnsAsync(oldJob);
            var mockLogger = new Mock<ILogger<JobsService>>();

            var jobService = new JobsService(mockRepo.Object, mockLogger.Object);

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

            var mockRepo = new Mock<IJobsRepository>();
            mockRepo.Setup(j => j.GetJobAsync(It.IsAny<int>())).ReturnsAsync(oldJob);
            mockRepo.Setup(j => j.UpdateJobAsync(It.IsAny<int>(),
                It.IsAny<JobWithoutStartTimeDto>())).ReturnsAsync(oldJob);
            var mockLogger = new Mock<ILogger<JobsService>>();

            var jobService = new JobsService(mockRepo.Object, mockLogger.Object);

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

            var mockRepo = new Mock<IJobsRepository>();
            mockRepo.Setup(j => j.GetJobAsync(It.IsAny<int>())).ReturnsAsync(oldJob);
            mockRepo.Setup(j => j.UpdateJobAsync(It.IsAny<int>(),
                It.IsAny<JobWithoutStartTimeDto>())).ReturnsAsync(updatedJob);
            var mockLogger = new Mock<ILogger<JobsService>>();

            var jobService = new JobsService(mockRepo.Object, mockLogger.Object);

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

            var mockRepo = new Mock<IJobsRepository>();
            mockRepo.Setup(j => j.GetJobAsync(It.IsAny<int>())).ReturnsAsync(oldJob);
            mockRepo.Setup(j => j.UpdateJobAsync(It.IsAny<int>(),
                It.IsAny<JobWithoutStartTimeDto>())).ReturnsAsync(updatedJob);
            var mockLogger = new Mock<ILogger<JobsService>>();

            var jobService = new JobsService(mockRepo.Object, mockLogger.Object);

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

            var mockRepo = new Mock<IJobsRepository>();
            mockRepo.Setup(j => j.GetJobAsync(It.IsAny<int>())).ReturnsAsync(finishedJob);
            mockRepo.Setup(j => j.FinishJobAsync(It.IsAny<int>())).ReturnsAsync(finishedJob);
            var mockLogger = new Mock<ILogger<JobsService>>();

            var jobService = new JobsService(mockRepo.Object, mockLogger.Object);

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

            var mockRepo = new Mock<IJobsRepository>();
            mockRepo.Setup(j => j.GetJobAsync(It.IsAny<int>())).ReturnsAsync(finishedJob);
            mockRepo.Setup(j => j.FinishJobAsync(It.IsAny<int>())).ReturnsAsync(finishedJob);
            var mockLogger = new Mock<ILogger<JobsService>>();

            var jobService = new JobsService(mockRepo.Object, mockLogger.Object);

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

            var mockRepo = new Mock<IJobsRepository>();
            mockRepo.Setup(j => j.GetJobAsync(It.IsAny<int>())).ReturnsAsync(jobBeforeFinish);
            mockRepo.Setup(j => j.FinishJobAsync(It.IsAny<int>())).ReturnsAsync(jobAfterFinish);
            var mockLogger = new Mock<ILogger<JobsService>>();

            var jobService = new JobsService(mockRepo.Object, mockLogger.Object);

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

            var mockRepo = new Mock<IJobsRepository>();
            mockRepo.Setup(j => j.GetJobAsync(It.IsAny<int>())).ReturnsAsync(jobBeforeFinish);
            mockRepo.Setup(j => j.FinishJobAsync(It.IsAny<int>())).ReturnsAsync(jobAfterFinish);
            var mockLogger = new Mock<ILogger<JobsService>>();

            var jobService = new JobsService(mockRepo.Object, mockLogger.Object);

            var result = await jobService.FinishJobAsync(jobId);

            Assert.Equal(jobAfterFinish, result.Job);
        }
    }
}
