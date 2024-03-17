
using Microsoft.Extensions.Configuration;
using MicroZoo.Infrastructure.Generals;
using MicroZoo.Infrastructure.MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi;
using MicroZoo.ZookeepersApi.Models;
using MicroZoo.ZookeepersApi.Services;

namespace MicroZoo.ZookeepersApi.Tests.UnitTests
{
    public class JobsRequestReceivingServiceTests
    {
        private Mock<IResponsesReceiverFromRabbitMq> _mockReceiver;
        private Mock<IJobsService> _mockJobsService;
        //private Mock<IConfiguration> _mockConfiguration;
        private Mock<IConnectionService> _mockConnection;

        public JobsRequestReceivingServiceTests()
        {
            _mockJobsService = new Mock<IJobsService>();
            _mockReceiver = new Mock<IResponsesReceiverFromRabbitMq>();
            //_mockConfiguration = new Mock<IConfiguration>();
            _mockConnection = new Mock<IConnectionService>();
        }

        [Theory]
        [MemberData(nameof(DateTimeRangeOrderingOptionsPageOptions))]
        public async void GetJobsForDateTimeRangeAsync_should_return_error_message_zookeeper_with_negative_id_doesnt_exist(
            DateTimeRange dateTimeRange, OrderingOptions orderingOptions, PageOptions pageOptions)
        {
            var expectedMessage = "Zookeeper with negative id doesn't exist";
            var negativeZookeeperId = new Fixture().Create<int>() * (-1);

            var personResponse = new GetPersonResponse() { ErrorMessage = expectedMessage };

            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<GetPersonRequest, GetPersonResponse>(
                It.IsAny<GetPersonRequest>(), It.IsAny<Uri>()))/*.ReturnsAsync(personResponse)*/;
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
            var ZookeeperId = new Fixture().Create<int>();
            dateTimeRange.StartDateTime = dateTimeRange.FinishDateTime.AddDays(1);

            var personResponse = new GetPersonResponse() { ErrorMessage = expectedMessage };

            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<GetPersonRequest, GetPersonResponse>(
                It.IsAny<GetPersonRequest>(), It.IsAny<Uri>()))/*.ReturnsAsync(personResponse)*/;
            _mockJobsService.Setup(s => s.GetJobsForDateTimeRangeAsync(It.IsAny<int>(), It.IsAny<DateTimeRange>(),
                It.IsAny<OrderingOptions>(), It.IsAny<PageOptions>()));

            var servise = new JobsRequestReceivingService(_mockJobsService.Object, _mockReceiver.Object, _mockConnection.Object);

            var result = await servise.GetJobsForDateTimeRangeAsync(ZookeeperId, dateTimeRange, orderingOptions, pageOptions);

            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Theory]
        [MemberData(nameof(DateTimeRangeOrderingOptionsPageOptions))]
        public async void GetJobsForDateTimeRangeAsync_should_return_error_message_zookeeper_with_id_does_not_exist(
            DateTimeRange dateTimeRange, OrderingOptions orderingOptions, PageOptions pageOptions)
        {
            //var expectedMessage = "Start time more or equals finish time";
            var ZookeeperId = new Fixture().Create<int>();
            dateTimeRange.StartDateTime = dateTimeRange.FinishDateTime.AddDays(1);

            var personResponse = new GetPersonResponse() { ErrorMessage = expectedMessage };

            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<GetPersonRequest, GetPersonResponse>(
                It.IsAny<GetPersonRequest>(), It.IsAny<Uri>()))/*.ReturnsAsync(personResponse)*/;
            _mockJobsService.Setup(s => s.GetJobsForDateTimeRangeAsync(It.IsAny<int>(), It.IsAny<DateTimeRange>(),
                It.IsAny<OrderingOptions>(), It.IsAny<PageOptions>()));

            var servise = new JobsRequestReceivingService(_mockJobsService.Object, _mockReceiver.Object, _mockConnection.Object);

            var result = await servise.GetJobsForDateTimeRangeAsync(ZookeeperId, dateTimeRange, orderingOptions, pageOptions);

            Assert.Equal(expectedMessage, result.ErrorMessage);
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
