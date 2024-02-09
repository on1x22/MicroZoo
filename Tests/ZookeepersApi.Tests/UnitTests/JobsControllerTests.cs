﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MicroZoo.Infrastructure.MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Jobs;
using MicroZoo.ZookeepersApi.Controllers;
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
        [Fact]
        public async void GetAllJobsOfZookeeper_should_return_error_message_unknown_error()
        {
            var expectedMessage = "Unknown error";
            int zookeeperId = new Fixture().Create<int>();
            //var _zookeepersApiUrl = new Fixture().Create<Uri>();
            /*List<Job> jobs = new List<Job>()
            {
                new Fixture().Build<Job>().With(j => j.ZookeeperId, zookeeperId).Create(),
                new Fixture().Build<Job>().With(j => j.ZookeeperId, zookeeperId).Create()
            };*/

            var jobsResponse = new GetJobsResponse() { ErrorMessage = expectedMessage};

            var mockReceiver = new Mock<IResponsesReceiverFromRabbitMq>();
            mockReceiver.Setup(r => r.GetResponseFromRabbitTask<GetAllJobsOfZookeeperRequest,
                GetJobsResponse>(It.IsAny<GetAllJobsOfZookeeperRequest>(), It.IsAny<Uri>()))
                .ReturnsAsync(jobsResponse);
            var mockConnnection = new Mock<IConnectionService>();
            
            var jobsController = new JobsController(mockReceiver.Object, mockConnnection.Object);

            var actionResult = await jobsController.GetAllJobsOfZookeeper(zookeeperId);

            Assert.IsType<BadRequestObjectResult>(actionResult);

            var result = (actionResult as BadRequestObjectResult).Value;
            Assert.Equal(expectedMessage, result);
        }

        [Fact]
        public async void GetAllJobsOfZookeeper_should_return_list_of_jobs_of_selected_zookeeper()
        {
            int zookeeperId = new Fixture().Create<int>();
            //var _zookeepersApiUrl = new Fixture().Create<Uri>();
            List<Job> jobs = new List<Job>()
            {
                new Fixture().Build<Job>().With(j => j.ZookeeperId, zookeeperId).Create(),
                new Fixture().Build<Job>().With(j => j.ZookeeperId, zookeeperId).Create()
            };

            var jobsResponse = new GetJobsResponse() { Jobs = jobs };

            var mockReceiver = new Mock<IResponsesReceiverFromRabbitMq>();
            mockReceiver.Setup(r => r.GetResponseFromRabbitTask<GetAllJobsOfZookeeperRequest,
                GetJobsResponse>(It.IsAny<GetAllJobsOfZookeeperRequest>(), It.IsAny<Uri>()))
                .ReturnsAsync(jobsResponse);
            var mockConnnection = new Mock<IConnectionService>();

            var jobsController = new JobsController(mockReceiver.Object, mockConnnection.Object);

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

            var mockReceiver = new Mock<IResponsesReceiverFromRabbitMq>();
            mockReceiver.Setup(r => r.GetResponseFromRabbitTask<GetCurrentJobsOfZookeeperRequest,
                GetJobsResponse>(It.IsAny<GetCurrentJobsOfZookeeperRequest>(), It.IsAny<Uri>()))
                .ReturnsAsync(jobsResponse);
            var mockConnnection = new Mock<IConnectionService>();

            var jobsController = new JobsController(mockReceiver.Object, mockConnnection.Object);

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

            var mockReceiver = new Mock<IResponsesReceiverFromRabbitMq>();
            mockReceiver.Setup(r => r.GetResponseFromRabbitTask<GetCurrentJobsOfZookeeperRequest,
                GetJobsResponse>(It.IsAny<GetCurrentJobsOfZookeeperRequest>(), It.IsAny<Uri>()))
                .ReturnsAsync(jobsResponse);
            var mockConnnection = new Mock<IConnectionService>();

            var jobsController = new JobsController(mockReceiver.Object, mockConnnection.Object);

            var actionResult = await jobsController.GetCurrentJobsOfZookeeper(zookeeperId);

            Assert.IsType<OkObjectResult>(actionResult);

            var result = actionResult.ReturnActionResultValue<OkObjectResult, List<Job>>();
            Assert.Equal(jobs, result);            
        }
    }
}
