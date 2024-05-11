using MicroZoo.Infrastructure.Generals;
using MicroZoo.Infrastructure.MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Jobs;
using MicroZoo.Infrastructure.Models.Jobs.Dto;
using MicroZoo.Infrastructure.Models.Persons;
using MicroZoo.ZookeepersApi.Services;

namespace MicroZoo.ZookeepersApi.Tests.UnitTests
{
    public class JobsRequestReceivingServiceTests
    {
        private Mock<IResponsesReceiverFromRabbitMq> _mockReceiver;
        private Mock<IJobsService> _mockJobsService;
        private Mock<IConnectionService> _mockConnection;

        public JobsRequestReceivingServiceTests()
        {
            _mockJobsService = new Mock<IJobsService>();
            _mockReceiver = new Mock<IResponsesReceiverFromRabbitMq>();
            _mockConnection = new Mock<IConnectionService>();
        }

        [Theory]
        [MemberData(nameof(DateTimeRangeOrderingOptionsPageOptions))]
        public async void GetJobsForDateTimeRangeAsync_should_return_error_message_zookeeper_id_must_be_more_than_0(
            DateTimeRange dateTimeRange, OrderingOptions orderingOptions, PageOptions pageOptions)
        {
            var expectedMessage = "Zookeeper Id must be more than 0";
            var negativeZookeeperId = new Fixture().Create<int>() * (-1);

            var personResponse = new GetPersonResponse() { ErrorMessage = expectedMessage };

            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<GetPersonRequest, GetPersonResponse>(
                It.IsAny<GetPersonRequest>(), It.IsAny<Uri>()));
            _mockJobsService.Setup(s => s.GetJobsForDateTimeRangeAsync(It.IsAny<int>(), It.IsAny<DateTimeRange>(),
                It.IsAny<OrderingOptions>(), It.IsAny<PageOptions>()));

            var servise = new JobsRequestReceivingService(_mockJobsService.Object, _mockReceiver.Object, _mockConnection.Object);

            var result = await servise.GetJobsForDateTimeRangeAsync(negativeZookeeperId, dateTimeRange, orderingOptions, pageOptions);

            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Theory]
        [MemberData(nameof(DateTimeRangeOrderingOptionsPageOptions))]
        public async void GetJobsForDateTimeRangeAsync_should_return_error_message_start_time_more_or_equals_finish_time(
            DateTimeRange dateTimeRange, OrderingOptions orderingOptions, PageOptions pageOptions)
        {
            var expectedMessage = "Start time more or equals finish time";
            var zookeeperId = new Fixture().Create<int>();
            dateTimeRange.StartDateTime = dateTimeRange.FinishDateTime.AddDays(1);

            var personResponse = new GetPersonResponse() { ErrorMessage = expectedMessage };

            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<GetPersonRequest, GetPersonResponse>(
                It.IsAny<GetPersonRequest>(), It.IsAny<Uri>()));
            _mockJobsService.Setup(s => s.GetJobsForDateTimeRangeAsync(It.IsAny<int>(), It.IsAny<DateTimeRange>(),
                It.IsAny<OrderingOptions>(), It.IsAny<PageOptions>()));

            var servise = new JobsRequestReceivingService(_mockJobsService.Object, _mockReceiver.Object, _mockConnection.Object);

            var result = await servise.GetJobsForDateTimeRangeAsync(zookeeperId, dateTimeRange, orderingOptions, pageOptions);

            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Theory]
        [MemberData(nameof(DateTimeRangeOrderingOptionsPageOptions))]
        public async void GetJobsForDateTimeRangeAsync_and_zookeeper_is_null_should_return_error_message_zookeeper_with_id_does_not_exist(
            DateTimeRange dateTimeRange, OrderingOptions orderingOptions, PageOptions pageOptions)
        {
            var zookeeperId = new Fixture().Create<int>();
            var expectedMessage = $"Zookeeper with id={zookeeperId} doesn't exist";

            var personResponse = new GetPersonResponse() { Person = null };

            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<GetPersonRequest, GetPersonResponse>(
                It.IsAny<GetPersonRequest>(), It.IsAny<Uri>())).ReturnsAsync(personResponse);
            _mockJobsService.Setup(s => s.GetJobsForDateTimeRangeAsync(It.IsAny<int>(), It.IsAny<DateTimeRange>(),
                It.IsAny<OrderingOptions>(), It.IsAny<PageOptions>()));

            var servise = new JobsRequestReceivingService(_mockJobsService.Object, _mockReceiver.Object, _mockConnection.Object);

            var result = await servise.GetJobsForDateTimeRangeAsync(zookeeperId, dateTimeRange, orderingOptions, pageOptions);

            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Theory]
        [MemberData(nameof(DateTimeRangeOrderingOptionsPageOptions))]
        public async void GetJobsForDateTimeRangeAsync_and_zookeeper_is_manager_should_return_error_message_zookeeper_with_id_does_not_exist(
            DateTimeRange dateTimeRange, OrderingOptions orderingOptions, PageOptions pageOptions)
        {
            var zookeeperId = new Fixture().Create<int>();
            var expectedMessage = $"Zookeeper with id={zookeeperId} doesn't exist";

            var personResponse = new GetPersonResponse() { Person = new Person() { IsManager = true } };

            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<GetPersonRequest, GetPersonResponse>(
                It.IsAny<GetPersonRequest>(), It.IsAny<Uri>())).ReturnsAsync(personResponse);
            _mockJobsService.Setup(s => s.GetJobsForDateTimeRangeAsync(It.IsAny<int>(), It.IsAny<DateTimeRange>(),
                It.IsAny<OrderingOptions>(), It.IsAny<PageOptions>()));

            var servise = new JobsRequestReceivingService(_mockJobsService.Object, _mockReceiver.Object, _mockConnection.Object);

            var result = await servise.GetJobsForDateTimeRangeAsync(zookeeperId, dateTimeRange, orderingOptions, pageOptions);

            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Theory]
        [MemberData(nameof(DateTimeRangeOrderingOptionsPageOptions))]
        public async void GetJobsForDateTimeRangeAsync_should_return_list_of_jobs(DateTimeRange dateTimeRange, 
            OrderingOptions orderingOptions, PageOptions pageOptions)
        {
            var zookeeperId = new Fixture().Create<int>();

            var person = new Fixture().Build<Person>().With(p => p.IsManager, false).Create();
            var personResponse = new Fixture().Build<GetPersonResponse>().With(r => r.Person, person).Create();
            
            var allJobs = JobsFactory.GetListOfAllJobs(zookeeperId);
            var selectedJobs = JobsFactory.GetJobsOfSelectedZookeeper(allJobs, zookeeperId);            
            var jobsResponse = new Fixture().Build<GetJobsResponse>().With(j => j.Jobs, selectedJobs).Create();

            var expectedJobs = allJobs.Where(j => j.ZookeeperId == zookeeperId).ToList();

            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<GetPersonRequest, GetPersonResponse>(
                It.IsAny<GetPersonRequest>(), It.IsAny<Uri>())).ReturnsAsync(personResponse);
            _mockJobsService.Setup(s => s.GetJobsForDateTimeRangeAsync(It.IsAny<int>(), It.IsAny<DateTimeRange>(),
                It.IsAny<OrderingOptions>(), It.IsAny<PageOptions>())).ReturnsAsync(jobsResponse);

            var servise = new JobsRequestReceivingService(_mockJobsService.Object, _mockReceiver.Object, _mockConnection.Object);

            var result = await servise.GetJobsForDateTimeRangeAsync(zookeeperId, dateTimeRange, orderingOptions, pageOptions);

            Assert.Equal(expectedJobs, result.Jobs);
        }

        [Fact]
        public async void AddJobAsync_should_return_error_message_zookeeper_id_must_be_more_than_0()
        {
            var expectedMessage = "Zookeeper Id must be more than 0";

            var jobDto = new Fixture().Build<JobDto>()
                .With(j => j.ZookeeperId, 0)
                .Create();

            var servise = new JobsRequestReceivingService(_mockJobsService.Object, _mockReceiver.Object, _mockConnection.Object);

            var result = await servise.AddJobAsync(jobDto);

            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void AddJobAsync_should_return_error_message_start_time_less_than_current_time()
        {
            var expectedMessage = "Start time less than current time";

            var jobDto = new Fixture().Build<JobDto>()
                .With(j => j.StartTime, DateTime.UtcNow.AddDays(-1))
                .Create();

            var servise = new JobsRequestReceivingService(_mockJobsService.Object, _mockReceiver.Object, _mockConnection.Object);

            var result = await servise.AddJobAsync(jobDto);

            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void AddJobAsync_should_return_error_message_deadline_didnt_set()
        {
            var expectedMessage = "Deadline didn't set";

            var jobDto = new Fixture().Build<JobDto>()
                .With(j => j.StartTime, DateTime.UtcNow.AddDays(1))
                .With(j => j.DeadlineTime, default(DateTime))
                .Create();

            var servise = new JobsRequestReceivingService(_mockJobsService.Object, _mockReceiver.Object, _mockConnection.Object);

            var result = await servise.AddJobAsync(jobDto);

            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void AddJobAsync_should_return_error_message_deadline_is_less_or_equal_start_time()
        {
            var expectedMessage = "Deadline is less or equal start time";
            var testDateTime = DateTime.UtcNow.AddDays(1);
            var jobDto = new Fixture().Build<JobDto>()
                .With(j => j.StartTime, testDateTime)
                .With(j => j.DeadlineTime, testDateTime)
                .Create();

            var servise = new JobsRequestReceivingService(_mockJobsService.Object, _mockReceiver.Object, _mockConnection.Object);

            var result = await servise.AddJobAsync(jobDto);

            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void AddJobAsync_should_return_error_message_priority_must_be_more_than_0()
        {
            var expectedMessage = "Priority must be more than 0";
            var testDateTime = DateTime.UtcNow.AddDays(1);
            var jobDto = new Fixture().Build<JobDto>()
                .With(j => j.StartTime, testDateTime)
                .With(j => j.DeadlineTime, testDateTime.AddDays(1))
                .With(j => j.Priority, 0)
                .Create();
                        
            var servise = new JobsRequestReceivingService(_mockJobsService.Object, _mockReceiver.Object, _mockConnection.Object);

            var result = await servise.AddJobAsync(jobDto);

            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void AddJobAsync_should_return_error_message_zookeeper_with_id_doesnt_exist()
        {
           
            var testDateTime = DateTime.UtcNow.AddDays(1);
            var jobDto = new Fixture().Build<JobDto>()
                .With(j => j.StartTime, testDateTime)
                .With(j => j.DeadlineTime, testDateTime.AddDays(1))
                .Create();
            
            var expectedMessage = $"Zookeeper with id={jobDto.ZookeeperId} doesn't exist";

            var personRespone = new Fixture().Build<GetPersonResponse>()
                .With(r => r.Person, (Person)null)
                .Create();
            
            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<GetPersonRequest, GetPersonResponse>(
                It.IsAny<GetPersonRequest>(), It.IsAny<Uri>())).ReturnsAsync(personRespone);
            
            var servise = new JobsRequestReceivingService(_mockJobsService.Object, _mockReceiver.Object, _mockConnection.Object);

            var result = await servise.AddJobAsync(jobDto);

            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void AddJobAsync_should_return_error_message()
        {
            var jobDto = new Fixture().Create<JobDto>();

            var jobResponse = new Fixture().Build<GetJobResponse>().With(r => r.Job, (Job)null).Create();
            var expectedMessage = jobResponse.ErrorMessage;

            var allJobs = JobsFactory.GetListOfAllJobs(jobDto.ZookeeperId);
            var selectedJobs = JobsFactory.GetJobsOfSelectedZookeeper(allJobs, jobDto.ZookeeperId);
            var jobsResponse = new Fixture().Build<GetJobsResponse>().With(j => j.Jobs, selectedJobs).Create();

            _mockJobsService.Setup(s => s.AddJobAsync(It.IsAny<JobDto>())).ReturnsAsync(jobResponse);
            _mockJobsService.Setup(s => s.GetCurrentJobsOfZookeeperAsync(It.IsAny<int>())).ReturnsAsync(jobsResponse);

            var servise = new JobsRequestReceivingService(_mockJobsService.Object, _mockReceiver.Object, _mockConnection.Object);

            var result = await servise.AddJobAsync(jobDto);

            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void AddJobAsync_should_return_list_of_jobs()
        {
            var jobDto = new Fixture().Create<JobDto>();

            var jobResponse = new Fixture().Build<GetJobResponse>().Create();
            var expectedMessage = jobResponse.ErrorMessage;

            var allJobs = JobsFactory.GetListOfAllJobs(jobResponse.Job.ZookeeperId);
            var selectedJobs = JobsFactory.GetJobsOfSelectedZookeeper(allJobs, jobResponse.Job.ZookeeperId);
            var jobsResponse = new Fixture().Build<GetJobsResponse>().With(j => j.Jobs, selectedJobs).Create();

            var expectedJobs = allJobs.Where(j => j.ZookeeperId == jobResponse.Job.ZookeeperId).ToList();

            _mockJobsService.Setup(s => s.AddJobAsync(It.IsAny<JobDto>())).ReturnsAsync(jobResponse);
            _mockJobsService.Setup(s => s.GetCurrentJobsOfZookeeperAsync(It.IsAny<int>())).ReturnsAsync(jobsResponse);

            var servise = new JobsRequestReceivingService(_mockJobsService.Object, _mockReceiver.Object, _mockConnection.Object);

            var result = await servise.AddJobAsync(jobDto);

            Assert.Equal(expectedJobs, result.Jobs);
        }

        [Fact]
        public async void UpdateJobAsync_should_return_error_message_task_with_negative_or_zero_id_does_not_exist()
        {
            var negativeJobId = new Fixture().Create<int>() * (-1);
            var jobDto = new Fixture().Create<JobWithoutStartTimeDto>();

            var expectedMessage = "Task with negative or zero id doesn't exist";

            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<GetPersonRequest, GetPersonResponse>(
                It.IsAny<GetPersonRequest>(), It.IsAny<Uri>()));
            _mockJobsService.Setup(s => s.UpdateJobAsync(It.IsAny<int>(), It.IsAny<JobWithoutStartTimeDto>()));
            _mockJobsService.Setup(s => s.GetCurrentJobsOfZookeeperAsync(It.IsAny<int>()));

            var servise = new JobsRequestReceivingService(_mockJobsService.Object, _mockReceiver.Object, _mockConnection.Object);

            var result = await servise.UpdateJobAsync(negativeJobId, jobDto);

            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void UpdateJobAsync_should_return_error_message_zookeeper_with_negative_or_zero_id_does_not_exist()
        {
            var jobId = new Fixture().Create<int>();
            var negativeZookeeperId = new Fixture().Create<int>() * (-1);
            var jobDto = new Fixture().Build<JobWithoutStartTimeDto>().With(d => d.ZookeeperId, negativeZookeeperId).Create();

            var expectedMessage = "Zookeeper with negative or zero id doesn't exist";

            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<GetPersonRequest, GetPersonResponse>(
                It.IsAny<GetPersonRequest>(), It.IsAny<Uri>()));
            _mockJobsService.Setup(s => s.UpdateJobAsync(It.IsAny<int>(), It.IsAny<JobWithoutStartTimeDto>()));
            _mockJobsService.Setup(s => s.GetCurrentJobsOfZookeeperAsync(It.IsAny<int>()));

            var servise = new JobsRequestReceivingService(_mockJobsService.Object, _mockReceiver.Object, _mockConnection.Object);

            var result = await servise.UpdateJobAsync(jobId, jobDto);

            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void UpdateJobAsync_should_return_error_message_task_description_must_consist_of_10_or_more_symbols()
        {
            var jobId = new Fixture().Create<int>();
            var jobDto = new Fixture().Create<JobWithoutStartTimeDto>();
            jobDto.Description = jobDto.Description[..9];

            var expectedMessage = "Task description must consist of 10 or more symbols";

            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<GetPersonRequest, GetPersonResponse>(
                It.IsAny<GetPersonRequest>(), It.IsAny<Uri>()));
            _mockJobsService.Setup(s => s.UpdateJobAsync(It.IsAny<int>(), It.IsAny<JobWithoutStartTimeDto>()));
            _mockJobsService.Setup(s => s.GetCurrentJobsOfZookeeperAsync(It.IsAny<int>()));

            var servise = new JobsRequestReceivingService(_mockJobsService.Object, _mockReceiver.Object, _mockConnection.Object);

            var result = await servise.UpdateJobAsync(jobId, jobDto);

            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void UpdateJobAsync_and_zookeeper_is_null_should_return_error_message_zookeeper_with_id_does_not_exist()
        {
            var jobId = new Fixture().Create<int>();
            var jobDto = new Fixture().Create<JobWithoutStartTimeDto>();
            
            var expectedMessage = $"Zookeeper with id={jobDto.ZookeeperId} doesn't exist";

            var personResponse = new GetPersonResponse() { Person = null };

            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<GetPersonRequest, GetPersonResponse>(
                It.IsAny<GetPersonRequest>(), It.IsAny<Uri>())).ReturnsAsync(personResponse);
            _mockJobsService.Setup(s => s.UpdateJobAsync(It.IsAny<int>(), It.IsAny<JobWithoutStartTimeDto>()));
            _mockJobsService.Setup(s => s.GetCurrentJobsOfZookeeperAsync(It.IsAny<int>()));

            var servise = new JobsRequestReceivingService(_mockJobsService.Object, _mockReceiver.Object, _mockConnection.Object);

            var result = await servise.UpdateJobAsync(jobId, jobDto);

            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void UpdateJobAsync_and_zookeeper_is_manager_should_return_error_message_zookeeper_with_id_does_not_exist()
        {
            var jobId = new Fixture().Create<int>();
            var jobDto = new Fixture().Create<JobWithoutStartTimeDto>();

            var expectedMessage = $"Zookeeper with id={jobDto.ZookeeperId} doesn't exist";

            var personResponse = new GetPersonResponse() { Person = new Person() { IsManager = true } };

            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<GetPersonRequest, GetPersonResponse>(
                It.IsAny<GetPersonRequest>(), It.IsAny<Uri>())).ReturnsAsync(personResponse);
            _mockJobsService.Setup(s => s.UpdateJobAsync(It.IsAny<int>(), It.IsAny<JobWithoutStartTimeDto>()));
            _mockJobsService.Setup(s => s.GetCurrentJobsOfZookeeperAsync(It.IsAny<int>()));

            var servise = new JobsRequestReceivingService(_mockJobsService.Object, _mockReceiver.Object, _mockConnection.Object);

            var result = await servise.UpdateJobAsync(jobId, jobDto);

            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void UpdateJobAsync_should_return_error_message_from_UpdateJobAsync_method()
        {
            var jobId = new Fixture().Create<int>();
            var jobDto = new Fixture().Create<JobWithoutStartTimeDto>();

            var expectedMessage = "Error message from _jobService.UpdateJobAsync()";

            var person = new Fixture().Build<Person>().With(p => p.IsManager, false).Create();
            var personResponse = new Fixture().Build<GetPersonResponse>().With(r => r.Person, person).Create();
            var jobResponse = new Fixture().Build<GetJobResponse>()
                .With(j => j.Job, (Job)null)
                .With(j => j.ErrorMessage, expectedMessage)
                .Create();

            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<GetPersonRequest, GetPersonResponse>(
                It.IsAny<GetPersonRequest>(), It.IsAny<Uri>())).ReturnsAsync(personResponse);
            _mockJobsService.Setup(s => s.UpdateJobAsync(It.IsAny<int>(), It.IsAny<JobWithoutStartTimeDto>()))
                .ReturnsAsync(jobResponse);
            _mockJobsService.Setup(s => s.GetCurrentJobsOfZookeeperAsync(It.IsAny<int>()));

            var servise = new JobsRequestReceivingService(_mockJobsService.Object, _mockReceiver.Object, _mockConnection.Object);

            var result = await servise.UpdateJobAsync(jobId, jobDto);

            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void UpdateJobAsync_should_return_list_of_jobs()
        {
            var jobId = new Fixture().Create<int>();
            var jobDto = new Fixture().Create<JobWithoutStartTimeDto>();

            var person = new Fixture().Build<Person>().With(p => p.IsManager, false).Create();
            var personResponse = new Fixture().Build<GetPersonResponse>().With(r => r.Person, person).Create();
            var jobResponse = new Fixture().Build<GetJobResponse>().Create();
            
            var allJobs = JobsFactory.GetListOfAllJobs(jobDto.ZookeeperId);
            var selectedJobs = JobsFactory.GetJobsOfSelectedZookeeper(allJobs, jobDto.ZookeeperId);
            var jobsResponse = new Fixture().Build<GetJobsResponse>().With(j => j.Jobs, selectedJobs).Create();

            var expectedJobs = allJobs.Where(j => j.ZookeeperId == jobDto.ZookeeperId).ToList();

            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<GetPersonRequest, GetPersonResponse>(
                It.IsAny<GetPersonRequest>(), It.IsAny<Uri>())).ReturnsAsync(personResponse);
            _mockJobsService.Setup(s => s.UpdateJobAsync(It.IsAny<int>(), It.IsAny<JobWithoutStartTimeDto>()))
                .ReturnsAsync(jobResponse);
            _mockJobsService.Setup(s => s.GetCurrentJobsOfZookeeperAsync(It.IsAny<int>())).ReturnsAsync(jobsResponse);

            var servise = new JobsRequestReceivingService(_mockJobsService.Object, _mockReceiver.Object, _mockConnection.Object);

            var result = await servise.UpdateJobAsync(jobId, jobDto);

            Assert.Equal(expectedJobs, result.Jobs);
        }

        [Fact]
        public async void FinishJobAsync_should_return_error_message_task_with_negative_or_zero_id_does_not_exist()
        {
            var jobId = new Fixture().Create<int>() * (-1);

            var expectedMessage = "Task with negative or zero id doesn't exist";

            _mockJobsService.Setup(s => s.FinishJobAsync(It.IsAny<int>()));
            _mockJobsService.Setup(s => s.GetCurrentJobsOfZookeeperAsync(It.IsAny<int>()));

            var servise = new JobsRequestReceivingService(_mockJobsService.Object, _mockReceiver.Object, _mockConnection.Object);

            var result = await servise.FinishJobAsync(jobId);

            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void FinishJobAsync_should_return_error_message_from_FinishJobAsync()
        {
            var jobId = new Fixture().Create<int>();

            var expectedMessage = "Error message from _jobService.FinishJobAsync()";

            var jobResponse = new Fixture().Build<GetJobResponse>()
                .With(r => r.Job, (Job)null)
                .With(r => r.ErrorMessage, expectedMessage)
                .Create();

            _mockJobsService.Setup(s => s.FinishJobAsync(It.IsAny<int>())).ReturnsAsync(jobResponse);
            _mockJobsService.Setup(s => s.GetCurrentJobsOfZookeeperAsync(It.IsAny<int>()));

            var servise = new JobsRequestReceivingService(_mockJobsService.Object, _mockReceiver.Object, _mockConnection.Object);

            var result = await servise.FinishJobAsync(jobId);

            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void FinishJobAsync_should_return_list_of_jobs()
        {
            var jobId = new Fixture().Create<int>();
            var zookeeperId = new Fixture().Create<int>();

            var allJobs = JobsFactory.GetListOfAllJobs(zookeeperId);
            var selectedJobs = JobsFactory.GetJobsOfSelectedZookeeper(allJobs, zookeeperId);
            var jobsResponse = new Fixture().Build<GetJobsResponse>().With(j => j.Jobs, selectedJobs).Create();

            var expectedJobs = allJobs.Where(j => j.ZookeeperId == zookeeperId).ToList();

            var jobResponse = new Fixture().Build<GetJobResponse>().Create();

            _mockJobsService.Setup(s => s.FinishJobAsync(It.IsAny<int>())).ReturnsAsync(jobResponse);
            _mockJobsService.Setup(s => s.GetCurrentJobsOfZookeeperAsync(It.IsAny<int>())).ReturnsAsync(jobsResponse);

            var servise = new JobsRequestReceivingService(_mockJobsService.Object, _mockReceiver.Object, _mockConnection.Object);

            var result = await servise.FinishJobAsync(jobId);

            Assert.Equal(expectedJobs, result.Jobs);
        }

        public static IEnumerable<object[]> DateTimeRangeOrderingOptionsPageOptions
        {
            get
            {
                yield return new object[]
                {
                    new Fixture().Build<DateTimeRange>()
                        .With(r => r.StartDateTime, DateTime.Now)
                        .With(r => r.FinishDateTime, DateTime.Now.AddDays(1))
                        .Create(),
                    new Fixture().Build<OrderingOptions>().Create(),
                    new Fixture().Build<PageOptions>().Create()
                };
            }
        }
    }
}
