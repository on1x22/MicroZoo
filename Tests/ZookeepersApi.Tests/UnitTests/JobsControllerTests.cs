using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MicroZoo.Infrastructure.MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Jobs;
using MicroZoo.Infrastructure.Models.Jobs.Dto;
using MicroZoo.ZookeepersApi.Controllers;
using MicroZoo.ZookeepersApi.Models;
using MicroZoo.ZookeepersApi.Services;
using MicroZoo.ZookeepersApi.Tests.UnitTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroZoo.ZookeepersApi.Tests.UnitTests
{
    public class JobsControllerTests
    {
        private Mock<IResponsesReceiverFromRabbitMq> _mockReceiver;
        private Mock<IConnectionService> _mockConnnection;

        public JobsControllerTests()
        {
            _mockReceiver = new Mock<IResponsesReceiverFromRabbitMq>();
            _mockConnnection = new Mock<IConnectionService>();
        }

        [Fact]
        public async void GetAllJobsOfZookeeper_should_return_error_message_unknown_error()
        {
            var expectedMessage = "Unknown error";
            int zookeeperId = new Fixture().Create<int>();
            
            var jobsResponse = new GetJobsResponse() { ErrorMessage = expectedMessage};

            //var mockReceiver = new Mock<IResponsesReceiverFromRabbitMq>();
            _mockReceiver.Setup(r => r.GetResponseFromRabbitTask<GetAllJobsOfZookeeperRequest,
                GetJobsResponse>(It.IsAny<GetAllJobsOfZookeeperRequest>(), It.IsAny<Uri>()))
                .ReturnsAsync(jobsResponse);
            //var mockConnnection = new Mock<IConnectionService>();
            
            var jobsController = new JobsController(_mockReceiver.Object, _mockConnnection.Object);

            var actionResult = await jobsController.GetAllJobsOfZookeeper(zookeeperId);

            Assert.IsType<BadRequestObjectResult>(actionResult);

            var result = (actionResult as BadRequestObjectResult).Value;
            Assert.Equal(expectedMessage, result);
        }

        [Fact]
        public async void GetAllJobsOfZookeeper_should_return_list_of_jobs_of_selected_zookeeper()
        {
            int zookeeperId = new Fixture().Create<int>();

            List<Job> jobs = new List<Job>()
            {
                new Fixture().Build<Job>().With(j => j.ZookeeperId, zookeeperId).Create(),
                new Fixture().Build<Job>().With(j => j.ZookeeperId, zookeeperId).Create()
            };

            var jobsResponse = new GetJobsResponse() { Jobs = jobs };

            //var mockReceiver = new Mock<IResponsesReceiverFromRabbitMq>();
            _mockReceiver.Setup(r => r.GetResponseFromRabbitTask<GetAllJobsOfZookeeperRequest,
                GetJobsResponse>(It.IsAny<GetAllJobsOfZookeeperRequest>(), It.IsAny<Uri>()))
                .ReturnsAsync(jobsResponse);
            //var mockConnnection = new Mock<IConnectionService>();

            var jobsController = new JobsController(_mockReceiver.Object, _mockConnnection.Object);

            var actionResult = await jobsController.GetAllJobsOfZookeeper(zookeeperId);

            Assert.IsType<OkObjectResult>(actionResult);

            var result = actionResult as OkObjectResult;

            var resultObject = result.Value as List<Job>;

            Assert.NotNull(resultObject);
            Assert.Equal(jobs, resultObject);
        }

        [Fact]
        public async void GetCurrentJobsOfZookeeper_should_return_error_message_unknown_error()
        {
            var expectedMessage = "Unknown error";
            int zookeeperId = new Fixture().Create<int>();

            var jobsResponse = new GetJobsResponse() { ErrorMessage = expectedMessage };

            //var mockReceiver = new Mock<IResponsesReceiverFromRabbitMq>();
            _mockReceiver.Setup(r => r.GetResponseFromRabbitTask<GetCurrentJobsOfZookeeperRequest,
                GetJobsResponse>(It.IsAny<GetCurrentJobsOfZookeeperRequest>(), It.IsAny<Uri>()))
                .ReturnsAsync(jobsResponse);
            //var mockConnnection = new Mock<IConnectionService>();

            var jobsController = new JobsController(_mockReceiver.Object, _mockConnnection.Object);

            var actionResult = await jobsController.GetCurrentJobsOfZookeeper(zookeeperId);

            var result = actionResult.ReturnActionResultValue<BadRequestObjectResult, string>();
            Assert.Equal(expectedMessage, result);
        }

        [Fact]
        public async void GetCurrentJobsOfZookeeper_should_return_list_of_current_jobs_of_selected_zookeeper()
        {
            int zookeeperId = new Fixture().Create<int>();
            DateTime? finishTime = null;

            List<Job> jobs = new List<Job>()
            {
                new Fixture().Build<Job>()
                    .With(j => j.ZookeeperId, zookeeperId)
                    .With(j => j.FinishTime, finishTime)
                    .Create(),
                new Fixture().Build<Job>()
                    .With(j => j.ZookeeperId, zookeeperId)
                    .With(j => j.FinishTime, finishTime)
                    .Create()
            };

            var jobsResponse = new GetJobsResponse() { Jobs = jobs };

            //var mockReceiver = new Mock<IResponsesReceiverFromRabbitMq>();
            _mockReceiver.Setup(r => r.GetResponseFromRabbitTask<GetCurrentJobsOfZookeeperRequest,
                GetJobsResponse>(It.IsAny<GetCurrentJobsOfZookeeperRequest>(), It.IsAny<Uri>()))
                .ReturnsAsync(jobsResponse);
            //var mockConnnection = new Mock<IConnectionService>();

            var jobsController = new JobsController(_mockReceiver.Object, _mockConnnection.Object);

            var actionResult = await jobsController.GetCurrentJobsOfZookeeper(zookeeperId);

            Assert.IsType<OkObjectResult>(actionResult);

            var result = actionResult.ReturnActionResultValue<OkObjectResult, List<Job>>();
            Assert.Equal(jobs, result);            
        }

        [Fact]
        public async void GetJobsForTimeRange_should_return_error_message_unknown_error()
        {
            var expectedMessage = "Unknown error";
            DateTime startDateTime = DateTime.Now.AddDays(-1);
            DateTime endDateTime = DateTime.Now.AddDays(1);
            int zookeeperId = new Fixture().Create<int>();

            var jobsResponse = new GetJobsResponse() { ErrorMessage = expectedMessage };

            //var mockReceiver = new Mock<IResponsesReceiverFromRabbitMq>();
            _mockReceiver.Setup(r => r.GetResponseFromRabbitTask<GetJobsForDateTimeRangeRequest,
                GetJobsResponse>(It.IsAny<GetJobsForDateTimeRangeRequest>(), It.IsAny<Uri>()))
                .ReturnsAsync(jobsResponse);
            //var mockConnnection = new Mock<IConnectionService>();

            var jobsController = new JobsController(_mockReceiver.Object, _mockConnnection.Object);

            var actionResult = await jobsController.GetJobsForTimeRange(startDateTime, endDateTime,
                zookeeperId);

            var result = actionResult.ReturnActionResultValue<BadRequestObjectResult, string>();
            Assert.Equal(expectedMessage, result);
        }

        [Fact]
        public async void GetJobsForTimeRange_should_return_list_of_jobs_of_all_zookeepers_for_time_range()
        {
            DateTime startDateTime = DateTime.Now.AddDays(-1);
            DateTime endDateTime = DateTime.Now.AddDays(1);

            List<Job> jobs = new List<Job>()
            {
                new Fixture().Build<Job>()
                    .With(j => j.StartTime, startDateTime)
                    .With(j => j.FinishTime, endDateTime)
                    .Create(),
                new Fixture().Build<Job>()
                    .With(j => j.StartTime, startDateTime)
                    .With(j => j.FinishTime, endDateTime)
                    .Create()
            };

            var jobsResponse = new GetJobsResponse() { Jobs = jobs };

            //var mockReceiver = new Mock<IResponsesReceiverFromRabbitMq>();
            _mockReceiver.Setup(r => r.GetResponseFromRabbitTask<GetJobsForDateTimeRangeRequest,
                GetJobsResponse>(It.IsAny<GetJobsForDateTimeRangeRequest>(), It.IsAny<Uri>()))
                .ReturnsAsync(jobsResponse);
            //var mockConnnection = new Mock<IConnectionService>();

            var jobsController = new JobsController(_mockReceiver.Object, _mockConnnection.Object);

            var actionResult = await jobsController.GetJobsForTimeRange(startDateTime, endDateTime);

            var result = actionResult.ReturnActionResultValue<OkObjectResult, List<Job>>();
            Assert.Equal(jobs, result);
        }

        [Fact]
        public async void GetJobsForTimeRange_should_return_list_of_jobs_of_selected_zookeeper_for_time_range()
        {
            DateTime startDateTime = DateTime.Now.AddDays(-1);
            DateTime endDateTime = DateTime.Now.AddDays(1);
            int zookeeperId = new Fixture().Create<int>();

            List<Job> jobs = new List<Job>()
            {
                new Fixture().Build<Job>()
                    .With(j => j.ZookeeperId, zookeeperId)
                    .With(j => j.StartTime, startDateTime)
                    .With(j => j.FinishTime, endDateTime)
                    .Create(),
                new Fixture().Build<Job>()
                    .With(j => j.ZookeeperId, zookeeperId)
                    .With(j => j.StartTime, startDateTime)
                    .With(j => j.FinishTime, endDateTime)
                    .Create()
            };

            var jobsResponse = new GetJobsResponse() { Jobs = jobs };

            //var mockReceiver = new Mock<IResponsesReceiverFromRabbitMq>();
            _mockReceiver.Setup(r => r.GetResponseFromRabbitTask<GetJobsForDateTimeRangeRequest,
                GetJobsResponse>(It.IsAny<GetJobsForDateTimeRangeRequest>(), It.IsAny<Uri>()))
                .ReturnsAsync(jobsResponse);
            //var mockConnnection = new Mock<IConnectionService>();

            var jobsController = new JobsController(_mockReceiver.Object, _mockConnnection.Object);

            var actionResult = await jobsController.GetJobsForTimeRange(startDateTime, endDateTime,
                zookeeperId);

            var result = actionResult.ReturnActionResultValue<OkObjectResult, List<Job>>();
            Assert.Equal(jobs, result);
        }

        [Fact]
        public async void AddJob_should_return_error_message_start_time_less_than_current_time()
        {
            var expectedMessage = "Start time less than current time";
            var jobDto = new Fixture().Build<JobDto>()
                .With(j => j.StartTime, DateTime.UtcNow.AddDays(-1))
                .Create();

            //var mockReceiver = new Mock<IResponsesReceiverFromRabbitMq>();
            //var mockConnnection = new Mock<IConnectionService>();

            var jobsController = new JobsController(_mockReceiver.Object, _mockConnnection.Object);

            var actionResult = await jobsController.AddJob(jobDto);

            var result = actionResult.ReturnActionResultValue<BadRequestObjectResult, string>();
            Assert.Equal(expectedMessage, result);
        }

        [Fact]
        public async void AddJob_should_return_error_message_unknown_error()
        {
            var expectedMessage = "Unknown error";
            var jobDto = new Fixture().Build<JobDto>()
                .With(j => j.StartTime, default(DateTime))
                .Create();

            var jobsResponse = new GetJobsResponse() { ErrorMessage = expectedMessage };

            //var mockReceiver = new Mock<IResponsesReceiverFromRabbitMq>();
            _mockReceiver.Setup(r => r.GetResponseFromRabbitTask<AddJobRequest,
                GetJobsResponse>(It.IsAny<AddJobRequest>(), It.IsAny<Uri>()))
                .ReturnsAsync(jobsResponse);
            //var mockConnnection = new Mock<IConnectionService>();

            var jobsController = new JobsController(_mockReceiver.Object, _mockConnnection.Object);

            var actionResult = await jobsController.AddJob(jobDto);

            var result = actionResult.ReturnActionResultValue<BadRequestObjectResult, string>();
            Assert.Equal(expectedMessage, result);
        }

        [Fact]
        public async void AddJob_should_return_list_of_current_jobs_of_zookeeper()
        {
            var jobDto = new Fixture().Build<JobDto>()
                .With(j => j.StartTime, default(DateTime))
                .Create();
            DateTime? endDateTime = null;

            List<Job> jobs = new List<Job>()
            {
                new Fixture().Build<Job>()
                    .With(j => j.ZookeeperId, jobDto.ZookeeperId)
                    .With(j => j.FinishTime, endDateTime)
                    .Create(),
                new Fixture().Build<Job>()
                    .With(j => j.ZookeeperId, jobDto.ZookeeperId)
                    .With(j => j.StartTime, jobDto.StartTime)
                    .With(j => j.FinishTime, endDateTime)
                    .With(j => j.Description, jobDto.Description)
                    .Create()
            };

            var jobsResponse = new GetJobsResponse() { Jobs = jobs };

            //var mockReceiver = new Mock<IResponsesReceiverFromRabbitMq>();
            _mockReceiver.Setup(r => r.GetResponseFromRabbitTask<AddJobRequest,
                GetJobsResponse>(It.IsAny<AddJobRequest>(), It.IsAny<Uri>()))
                .ReturnsAsync(jobsResponse);
            //var mockConnnection = new Mock<IConnectionService>();

            var jobsController = new JobsController(_mockReceiver.Object, _mockConnnection.Object);

            var actionResult = await jobsController.AddJob(jobDto);

            var result = actionResult.ReturnActionResultValue<OkObjectResult, List<Job>>();
            Assert.Equal(jobs, result);
        }

        [Fact]
        public async void UpdateJob_should_return_error_message_unknown_error()
        {
            var expectedMessage = "Unknown error";
            int jobId = new Fixture().Create<int>();
            var jobDto = new Fixture().Build<JobWithoutStartTimeDto>().Create();

            var jobsResponse = new GetJobsResponse() { ErrorMessage = expectedMessage };

            //var mockReceiver = new Mock<IResponsesReceiverFromRabbitMq>();
            _mockReceiver.Setup(r => r.GetResponseFromRabbitTask<UpdateJobRequest,
                GetJobsResponse>(It.IsAny<UpdateJobRequest>(), It.IsAny<Uri>()))
                .ReturnsAsync(jobsResponse);
            //var mockConnnection = new Mock<IConnectionService>();

            var jobsController = new JobsController(_mockReceiver.Object, _mockConnnection.Object);

            var actionResult = await jobsController.UpdateJob(jobId, jobDto);

            var result = actionResult.ReturnActionResultValue<BadRequestObjectResult, string>();
            Assert.Equal(expectedMessage, result);
        }

        [Fact]
        public async void UpdateJob_should_return_list_of_current_jobs_of_zookeeper()
        {
            int jobId = new Fixture().Create<int>();
            var jobDto = new Fixture().Build<JobWithoutStartTimeDto>().Create();

            DateTime? endDateTime = null;

            List<Job> jobs = new List<Job>()
            {
                new Fixture().Build<Job>()
                    .With(j => j.ZookeeperId, jobDto.ZookeeperId)
                    .With(j => j.FinishTime, endDateTime)
                    .Create(),
                new Fixture().Build<Job>()
                    .With(j => j.ZookeeperId, jobDto.ZookeeperId)
                    .With(j => j.FinishTime, endDateTime)
                    .With(j => j.Description, jobDto.Description)
                    .Create()
            };

            var jobsResponse = new GetJobsResponse() { Jobs = jobs };

            //var mockReceiver = new Mock<IResponsesReceiverFromRabbitMq>();
            _mockReceiver.Setup(r => r.GetResponseFromRabbitTask<UpdateJobRequest,
                GetJobsResponse>(It.IsAny<UpdateJobRequest>(), It.IsAny<Uri>()))
                .ReturnsAsync(jobsResponse);
            //var mockConnnection = new Mock<IConnectionService>();

            var jobsController = new JobsController(_mockReceiver.Object, _mockConnnection.Object);

            var actionResult = await jobsController.UpdateJob(jobId, jobDto);

            var result = actionResult.ReturnActionResultValue<OkObjectResult, List<Job>>();
            Assert.Equal(jobs, result);
        }

        [Fact]
        public async void FinishJob_should_return_error_message_unknown_error()
        {
            var expectedMessage = "Unknown error";
            int jobId = new Fixture().Create<int>();

            var jobsResponse = new GetJobsResponse() { ErrorMessage = expectedMessage };

            //var mockReceiver = new Mock<IResponsesReceiverFromRabbitMq>();
            _mockReceiver.Setup(r => r.GetResponseFromRabbitTask<FinishJobRequest,
                GetJobsResponse>(It.IsAny<FinishJobRequest>(), It.IsAny<Uri>()))
                .ReturnsAsync(jobsResponse);
            //var mockConnnection = new Mock<IConnectionService>();

            var jobsController = new JobsController(_mockReceiver.Object, _mockConnnection.Object);

            var actionResult = await jobsController.FinishJob(jobId);

            var result = actionResult.ReturnActionResultValue<BadRequestObjectResult, string>();
            Assert.Equal(expectedMessage, result);
        }

        [Fact]
        public async void FinishJob_should_return_list_of_current_jobs_of_zookeeper()
        {
            int jobId = new Fixture().Create<int>();

            DateTime? endDateTime = null;

            List<Job> jobs = new List<Job>()
            {
                new Fixture().Build<Job>()
                    .With(j => j.FinishTime, endDateTime)
                    .Create(),
                new Fixture().Build<Job>()
                    .With(j => j.FinishTime, endDateTime)
                    .Create()
            };

            var jobsResponse = new GetJobsResponse() { Jobs = jobs };

            //var mockReceiver = new Mock<IResponsesReceiverFromRabbitMq>();
            _mockReceiver.Setup(r => r.GetResponseFromRabbitTask<FinishJobRequest,
                GetJobsResponse>(It.IsAny<FinishJobRequest>(), It.IsAny<Uri>()))
                .ReturnsAsync(jobsResponse);
            //var mockConnnection = new Mock<IConnectionService>();

            var jobsController = new JobsController(_mockReceiver.Object, _mockConnnection.Object);

            var actionResult = await jobsController.FinishJob(jobId);

            var result = actionResult.ReturnActionResultValue<OkObjectResult, List<Job>>();
            Assert.Equal(jobs, result);
        }
    }
}
