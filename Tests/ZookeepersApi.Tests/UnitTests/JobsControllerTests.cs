﻿using Microsoft.AspNetCore.Mvc;
using MicroZoo.AuthService.Services;
using MicroZoo.Infrastructure.Generals;
using MicroZoo.Infrastructure.MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Jobs;
using MicroZoo.Infrastructure.Models.Jobs.Dto;
using MicroZoo.ZookeepersApi.Controllers;
using MicroZoo.ZookeepersApi.Services;

namespace MicroZoo.ZookeepersApi.Tests.UnitTests
{
    public class JobsControllerTests
    {
        private Mock<IResponsesReceiverFromRabbitMq> _mockReceiver;
        private Mock<IConnectionService> _mockConnnection;
        private Mock<IJobsRequestReceivingService> _mockReceivingService;
        private Mock<IRabbitMqResponseErrorsHandler> _mockErrorsHandler;
        private Mock<IAuthorizationService> _mockAuthorizationService;

        public JobsControllerTests()
        {
            _mockReceiver = new Mock<IResponsesReceiverFromRabbitMq>();
            _mockConnnection = new Mock<IConnectionService>();
            _mockReceivingService = new Mock<IJobsRequestReceivingService>();
            _mockErrorsHandler = new Mock<IRabbitMqResponseErrorsHandler>();
            _mockAuthorizationService = new Mock<IAuthorizationService>();
        }

        [Fact]
        public async void GetAllJobsOfZookeeper_should_return_error_message_unknown_error()
        {
            var expectedMessage = "Unknown error";
            int zookeeperId = new Fixture().Create<int>();
            
            var jobsResponse = new GetJobsResponse() { ErrorMessage = expectedMessage};

            /*_mockReceiver.Setup(r => r.GetResponseFromRabbitTask<GetAllJobsOfZookeeperRequest,
                GetJobsResponse>(It.IsAny<GetAllJobsOfZookeeperRequest>(), It.IsAny<Uri>()))
                .ReturnsAsync(jobsResponse);*/

            _mockReceivingService.Setup(rs => rs.GetAllJobsOfZookeeperAsync(It.IsAny<int>()))
                .ReturnsAsync(jobsResponse);

            var jobsController = new JobsController(_mockReceivingService.Object,
                                                    _mockAuthorizationService.Object,
                                                    _mockConnnection.Object,
                                                    _mockErrorsHandler.Object);

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

            /*_mockReceiver.Setup(r => r.GetResponseFromRabbitTask<GetAllJobsOfZookeeperRequest,
                GetJobsResponse>(It.IsAny<GetAllJobsOfZookeeperRequest>(), It.IsAny<Uri>()))
                .ReturnsAsync(jobsResponse);*/

            _mockReceivingService.Setup(rs => rs.GetAllJobsOfZookeeperAsync(It.IsAny<int>()))
                .ReturnsAsync(jobsResponse);

            var jobsController = new JobsController(_mockReceivingService.Object,
                                                    _mockAuthorizationService.Object,
                                                    _mockConnnection.Object,
                                                    _mockErrorsHandler.Object);

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

            /*_mockReceiver.Setup(r => r.GetResponseFromRabbitTask<GetCurrentJobsOfZookeeperRequest,
                GetJobsResponse>(It.IsAny<GetCurrentJobsOfZookeeperRequest>(), It.IsAny<Uri>()))
                .ReturnsAsync(jobsResponse);*/

            _mockReceivingService.Setup(rs => rs.GetCurrentJobsOfZookeeperAsync(It.IsAny<int>()))
                .ReturnsAsync(jobsResponse);

            var jobsController = new JobsController(_mockReceivingService.Object,
                                                    _mockAuthorizationService.Object,
                                                    _mockConnnection.Object,
                                                    _mockErrorsHandler.Object);

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

            /*_mockReceiver.Setup(r => r.GetResponseFromRabbitTask<GetCurrentJobsOfZookeeperRequest,
                GetJobsResponse>(It.IsAny<GetCurrentJobsOfZookeeperRequest>(), It.IsAny<Uri>()))
                .ReturnsAsync(jobsResponse);*/

            _mockReceivingService.Setup(rs => rs.GetCurrentJobsOfZookeeperAsync(It.IsAny<int>()))
                .ReturnsAsync(jobsResponse);

            var jobsController = new JobsController(_mockReceivingService.Object,
                                                    _mockAuthorizationService.Object,
                                                    _mockConnnection.Object,
                                                    _mockErrorsHandler.Object);

            var actionResult = await jobsController.GetCurrentJobsOfZookeeper(zookeeperId);

            Assert.IsType<OkObjectResult>(actionResult);

            var result = actionResult.ReturnActionResultValue<OkObjectResult, List<Job>>();
            Assert.Equal(jobs, result);            
        }

        [Fact]
        public async void GetJobsForTimeRange_should_return_error_message_unknown_error()
        {
            var expectedMessage = "Unknown error";
            int zookeeperId = new Fixture().Create<int>();
            DateTime startDateTime = DateTime.Now.AddDays(-1);
            DateTime endDateTime = DateTime.Now.AddDays(1);
            string propertyName = new Fixture().Create<string>();
            bool orderDescending = new Fixture().Create<bool>();
            int pageNumber = new Fixture().Create<int>();
            int itemsOnPage = new Fixture().Create<int>();

            var jobsResponse = new GetJobsResponse() { ErrorMessage = expectedMessage };

            /*_mockReceiver.Setup(r => r.GetResponseFromRabbitTask<GetJobsForDateTimeRangeRequest,
                GetJobsResponse>(It.IsAny<GetJobsForDateTimeRangeRequest>(), It.IsAny<Uri>()))
                .ReturnsAsync(jobsResponse);*/

            _mockReceivingService.Setup(rs => rs.GetJobsForDateTimeRangeAsync(It.IsAny<int>(),
                It.IsAny<DateTimeRange>(), It.IsAny<OrderingOptions>(), It.IsAny<PageOptions>(),
                It.IsAny<string>()))
                .ReturnsAsync(jobsResponse);

            var jobsController = new JobsController(_mockReceivingService.Object,
                                                    _mockAuthorizationService.Object,
                                                    _mockConnnection.Object,
                                                    _mockErrorsHandler.Object);

            var actionResult = await jobsController.GetJobsForTimeRange(startDateTime, endDateTime, 
                zookeeperId, propertyName, orderDescending, pageNumber, itemsOnPage);

            var result = actionResult.ReturnActionResultValue<BadRequestObjectResult, string>();
            Assert.Equal(expectedMessage, result);
        }

        [Fact]
        public async void GetJobsForTimeRange_should_return_list_of_jobs_of_all_zookeepers_for_time_range()
        {
            int zookeeperId = new Fixture().Create<int>();
            DateTime startDateTime = DateTime.Now.AddDays(-1);
            DateTime endDateTime = DateTime.Now.AddDays(1);
            string propertyName = new Fixture().Create<string>();
            bool orderDescending = new Fixture().Create<bool>();
            int pageNumber = new Fixture().Create<int>();
            int itemsOnPage = new Fixture().Create<int>();

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

            /*_mockReceiver.Setup(r => r.GetResponseFromRabbitTask<GetJobsForDateTimeRangeRequest,
                GetJobsResponse>(It.IsAny<GetJobsForDateTimeRangeRequest>(), It.IsAny<Uri>()))
                .ReturnsAsync(jobsResponse);*/

            _mockReceivingService.Setup(rs => rs.GetJobsForDateTimeRangeAsync(It.IsAny<int>(),
                It.IsAny<DateTimeRange>(), It.IsAny<OrderingOptions>(), It.IsAny<PageOptions>(),
                It.IsAny<string>()))
                .ReturnsAsync(jobsResponse);

            var jobsController = new JobsController(_mockReceivingService.Object,
                                                    _mockAuthorizationService.Object,
                                                    _mockConnnection.Object,
                                                    _mockErrorsHandler.Object);

            var actionResult = await jobsController.GetJobsForTimeRange(startDateTime, endDateTime, 
                zookeeperId, propertyName, orderDescending, pageNumber, itemsOnPage);

            var result = actionResult.ReturnActionResultValue<OkObjectResult, List<Job>>();
            Assert.Equal(jobs, result);
        }

        [Fact]
        public async void GetJobsForTimeRange_should_return_list_of_jobs_of_selected_zookeeper_for_time_range()
        {
            int zookeeperId = new Fixture().Create<int>();
            DateTime startDateTime = DateTime.Now.AddDays(-1);
            DateTime endDateTime = DateTime.Now.AddDays(1);            
            string propertyName = new Fixture().Create<string>();
            bool orderDescending = new Fixture().Create<bool>();
            int pageNumber = new Fixture().Create<int>();
            int itemsOnPage = new Fixture().Create<int>();

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

            /*_mockReceiver.Setup(r => r.GetResponseFromRabbitTask<GetJobsForDateTimeRangeRequest,
                GetJobsResponse>(It.IsAny<GetJobsForDateTimeRangeRequest>(), It.IsAny<Uri>()))
                .ReturnsAsync(jobsResponse);*/

            _mockReceivingService.Setup(rs => rs.GetJobsForDateTimeRangeAsync(It.IsAny<int>(),
                It.IsAny<DateTimeRange>(), It.IsAny<OrderingOptions>(), It.IsAny<PageOptions>(),
                It.IsAny<string>()))
                .ReturnsAsync(jobsResponse);

            var jobsController = new JobsController(_mockReceivingService.Object,
                                                    _mockAuthorizationService.Object,
                                                    _mockConnnection.Object,
                                                    _mockErrorsHandler.Object);

            var actionResult = await jobsController.GetJobsForTimeRange(startDateTime, endDateTime, 
                zookeeperId, propertyName, orderDescending, pageNumber, itemsOnPage);

            var result = actionResult.ReturnActionResultValue<OkObjectResult, List<Job>>();
            Assert.Equal(jobs, result);
        }

        /*[Fact]
        public async void AddJob_should_return_error_message_zookeeper_id_must_be_more_that_0()
        {
            var expectedMessage = "Zookeeper Id must be more that 0";
            var jobDto = new Fixture().Build<JobDto>()
                .With(j => j.ZookeeperId, 0)
                .Create();

            var jobsController = new JobsController(_mockReceiver.Object, _mockConnnection.Object,
                _mockReceivingService.Object);

            var actionResult = await jobsController.AddJob(jobDto);

            var result = actionResult.ReturnActionResultValue<BadRequestObjectResult, string>();
            Assert.Equal(expectedMessage, result);
        }*/

        /*[Fact]
        public async void AddJob_should_return_error_message_start_time_less_than_current_time()
        {
            var expectedMessage = "Start time less than current time";
            var jobDto = new Fixture().Build<JobDto>()
                .With(j => j.StartTime, DateTime.UtcNow.AddDays(-1))
                .Create();

            var jobsController = new JobsController(_mockReceiver.Object, _mockConnnection.Object,
                _mockReceivingService.Object);

            var actionResult = await jobsController.AddJob(jobDto);

            var result = actionResult.ReturnActionResultValue<BadRequestObjectResult, string>();
            Assert.Equal(expectedMessage, result);
        }*/

        /*[Fact]
        public async void AddJob_should_return_error_message_deadline_didnt_set()
        {
            var expectedMessage = "Deadline didn't set";
            var jobDto = new Fixture().Build<JobDto>()
                .With(j => j.StartTime, DateTime.UtcNow.AddDays(1))
                .With(j => j.DeadlineTime, default(DateTime))
                .Create();

            var jobsController = new JobsController(_mockReceiver.Object, _mockConnnection.Object,
                _mockReceivingService.Object);

            var actionResult = await jobsController.AddJob(jobDto);

            var result = actionResult.ReturnActionResultValue<BadRequestObjectResult, string>();
            Assert.Equal(expectedMessage, result);
        }*/

        /*[Fact]
        public async void AddJob_should_return_error_message_deadline_is_less_or_equal_start_time()
        {
            var expectedMessage = "Deadline is less or equal start time";
            var testDateTime = DateTime.UtcNow.AddDays(1);
            var jobDto = new Fixture().Build<JobDto>()
                .With(j => j.StartTime, testDateTime)
                .With(j => j.DeadlineTime, testDateTime)
                .Create();

            var jobsController = new JobsController(_mockReceiver.Object, _mockConnnection.Object,
                _mockReceivingService.Object);

            var actionResult = await jobsController.AddJob(jobDto);

            var result = actionResult.ReturnActionResultValue<BadRequestObjectResult, string>();
            Assert.Equal(expectedMessage, result);
        }*/

        /*[Fact]
        public async void AddJob_should_return_error_message_priority_must_be_more_than_0()
        {
            var expectedMessage = "Priority must be more than 0";
            var testDateTime = DateTime.UtcNow.AddDays(1);
            var jobDto = new Fixture().Build<JobDto>()
                .With(j => j.StartTime, testDateTime)
                .With(j => j.DeadlineTime, testDateTime.AddDays(1))
                .With(j => j.Priority, 0)
                .Create();

            var jobsController = new JobsController(_mockReceiver.Object, _mockConnnection.Object,
                _mockReceivingService.Object);

            var actionResult = await jobsController.AddJob(jobDto);

            var result = actionResult.ReturnActionResultValue<BadRequestObjectResult, string>();
            Assert.Equal(expectedMessage, result);
        }*/

        [Fact]
        public async void AddJob_should_return_error_message_unknown_error()
        {
            var expectedMessage = "Unknown error";
            var jobDto = new Fixture().Build<JobDto>()
                .With(j => j.StartTime, default(DateTime))
                .Create();

            var jobsResponse = new GetJobsResponse() { ErrorMessage = expectedMessage };

            /*_mockReceiver.Setup(r => r.GetResponseFromRabbitTask<AddJobRequest,
                GetJobsResponse>(It.IsAny<AddJobRequest>(), It.IsAny<Uri>()))
                .ReturnsAsync(jobsResponse);*/

            _mockReceivingService.Setup(rs => rs.AddJobAsync(jobDto, It.IsAny<string>()))
                .ReturnsAsync(jobsResponse);

            var jobsController = new JobsController(_mockReceivingService.Object,
                                                    _mockAuthorizationService.Object,
                                                    _mockConnnection.Object,
                                                    _mockErrorsHandler.Object);

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

            /*_mockReceiver.Setup(r => r.GetResponseFromRabbitTask<AddJobRequest,
                GetJobsResponse>(It.IsAny<AddJobRequest>(), It.IsAny<Uri>()))
                .ReturnsAsync(jobsResponse);*/

            _mockReceivingService.Setup(rs => rs.AddJobAsync(jobDto, It.IsAny<string>()))
                .ReturnsAsync(jobsResponse);

            var jobsController = new JobsController(_mockReceivingService.Object,
                                                    _mockAuthorizationService.Object,
                                                    _mockConnnection.Object,
                                                    _mockErrorsHandler.Object);

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

            var jobsResponse = new Fixture().Build<GetJobsResponse>()
                .With(r => r.Jobs, (List<Job>)null)
                .With(r => r.ErrorMessage, expectedMessage)
                .Create();
               
            _mockReceivingService.Setup(rs => rs.UpdateJobAsync(jobId, jobDto, It.IsAny<string>()))
                .ReturnsAsync(jobsResponse);
            
            var jobsController = new JobsController(_mockReceivingService.Object,
                                                    _mockAuthorizationService.Object,
                                                    _mockConnnection.Object,
                                                    _mockErrorsHandler.Object);

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

            _mockReceivingService.Setup(rs => rs.UpdateJobAsync(jobId, jobDto, It.IsAny<string>()))
                .ReturnsAsync(jobsResponse);

            var jobsController = new JobsController(_mockReceivingService.Object,
                                                    _mockAuthorizationService.Object,
                                                    _mockConnnection.Object,
                                                    _mockErrorsHandler.Object);

            var actionResult = await jobsController.UpdateJob(jobId, jobDto);

            var result = actionResult.ReturnActionResultValue<OkObjectResult, List<Job>>();
            Assert.Equal(jobs, result);
        }

        [Fact]
        public async void FinishJob_should_return_error_message_unknown_error()
        {
            var expectedMessage = "Unknown error";
            int jobId = new Fixture().Create<int>();
            var jobReport = new Fixture().Create<string>();

            var jobsResponse = new GetJobsResponse() { ErrorMessage = expectedMessage };

            /*_mockReceiver.Setup(r => r.GetResponseFromRabbitTask<FinishJobRequest,
                GetJobsResponse>(It.IsAny<FinishJobRequest>(), It.IsAny<Uri>()))
                .ReturnsAsync(jobsResponse);*/

            _mockReceivingService.Setup(rs => rs.FinishJobAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(jobsResponse);

            var jobsController = new JobsController(_mockReceivingService.Object,
                                                    _mockAuthorizationService.Object,
                                                    _mockConnnection.Object,
                                                    _mockErrorsHandler.Object);

            var actionResult = await jobsController.FinishJob(jobId, jobReport);

            var result = actionResult.ReturnActionResultValue<BadRequestObjectResult, string>();
            Assert.Equal(expectedMessage, result);
        }

        [Fact]
        public async void FinishJob_should_return_list_of_current_jobs_of_zookeeper()
        {
            int jobId = new Fixture().Create<int>();
            var jobReport = new Fixture().Create<string>();

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

            /*_mockReceiver.Setup(r => r.GetResponseFromRabbitTask<FinishJobRequest,
                GetJobsResponse>(It.IsAny<FinishJobRequest>(), It.IsAny<Uri>()))
                .ReturnsAsync(jobsResponse);*/

            _mockReceivingService.Setup(rs => rs.FinishJobAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(jobsResponse);

            var jobsController = new JobsController(_mockReceivingService.Object,
                                                    _mockAuthorizationService.Object,
                                                    _mockConnnection.Object,
                                                    _mockErrorsHandler.Object);

            var actionResult = await jobsController.FinishJob(jobId, jobReport);

            var result = actionResult.ReturnActionResultValue<OkObjectResult, List<Job>>();
            Assert.Equal(jobs, result);
        }
    }
}
