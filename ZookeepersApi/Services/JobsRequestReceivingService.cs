using MassTransit;
using MicroZoo.Infrastructure.MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Jobs.Dto;
using MicroZoo.ZookeepersApi.Models;
using System.Net;

namespace MicroZoo.ZookeepersApi.Services
{
    public class JobsRequestReceivingService : IJobsRequestReceivingService
    {
        private readonly IJobsService _jobService;
        private readonly IResponsesReceiverFromRabbitMq _receiver;
        private readonly Uri _animalsApiUrl;
        private readonly Uri _personsApiUrl;
        private readonly Uri _zookeepersApiUrl;

        public JobsRequestReceivingService(IJobsService jobService, IResponsesReceiverFromRabbitMq receiver,
            IConfiguration configuration)
        {
            _jobService = jobService;
            _receiver = receiver;
            _animalsApiUrl = new Uri(configuration["ConnectionStrings:AnimalsApiRmq"]);
            _personsApiUrl = new Uri(configuration["ConnectionStrings:PersonsApiRmq"]);
            _zookeepersApiUrl = new Uri(configuration["ConnectionStrings:ZookeepersApiRmq"]);
        }

        public async Task<GetJobsResponse> GetAllJobsOfZookeeperAsync(int zookeeperId)
        {
            return await _jobService.GetAllJobsOfZookeeperAsync(zookeeperId);
        }

        public async Task<GetJobsResponse> GetCurrentJobsOfZookeeperAsync(int zookeeperId)
        {
            return await _jobService.GetCurrentJobsOfZookeeperAsync(zookeeperId);
        }

        public async Task<GetJobsResponse> GetJobsForTimeRangeAsync(int zookeeperId, 
            DateTime startDateTime, DateTime finishDateTime)
        {
            var response = new GetJobsResponse();

            if (zookeeperId < 0)
            {
                response.ErrorMessage = "Zookeeper with negative id doesn't exist";
                return response;
            }

            if (finishDateTime == default)
                finishDateTime = DateTime.MaxValue;

            if (startDateTime >= finishDateTime)
            {
                response.ErrorMessage = "Start time more or equals finish time";
                return response;
            }

            if (zookeeperId > 0)
            {
                var personResponse = await _receiver.GetResponseFromRabbitTask<GetPersonRequest,
                    GetPersonResponse>(new GetPersonRequest(zookeeperId), _personsApiUrl);

                if (personResponse.Person == null || personResponse.Person.IsManager == true)
                {
                    response.ErrorMessage = $"Zookeeper with id={zookeeperId} doesn't exist";
                    return response;
                }
            }

            response = await _jobService.GetJobsForTimeRangeAsync(zookeeperId, startDateTime, 
                finishDateTime);

            return response;
        }

        public async Task<GetJobsResponse> AddJobAsync(JobDto jobDto)
        {
            var jobResponse = await _jobService.AddJobAsync(jobDto);

            var response = new GetJobsResponse();

            if (jobResponse.Job == null)
            {
                response.ErrorMessage = jobResponse.ErrorMessage;
                return response;
            }

            //response = await _receiver.GetResponseFromRabbitTask<GetCurrentJobsOfZookeeperRequest,
            //    GetJobsResponse>(new GetCurrentJobsOfZookeeperRequest(jobDto.ZookeeperId), 
            //    _zookeepersApiUrl);
            response = await _jobService.GetCurrentJobsOfZookeeperAsync(jobDto.ZookeeperId);

            return response;
        }

        public async Task<GetJobsResponse> UpdateJobAsync(int jobId, JobWithoutStartTimeDto jobDto)
        {
            var response = new GetJobsResponse();

            if (jobId <= 0)
            {
                response.ErrorMessage = "Task with negative or zero id doesn't exist";
                return response;
            }

            if (jobDto.ZookeeperId <= 0)
            {
                response.ErrorMessage = "Zookeeper with negative or zero id doesn't exist";
                return response;
            }

            if (jobDto.Description.Length < 10)
            {
                response.ErrorMessage = "Task description must consist of 10 or more symbols";
                return response;
            }

            var personResponse = await _receiver.GetResponseFromRabbitTask<GetPersonRequest,
                    GetPersonResponse>(new GetPersonRequest(jobDto.ZookeeperId), _personsApiUrl);

            if (personResponse.Person == null || personResponse.Person.IsManager == true)
            {
                response.ErrorMessage = $"Zookeeper with id={jobDto.ZookeeperId} doesn't exist";
                return response;
            }

            var jobResponse = await _jobService.UpdateJobAsync(jobId, jobDto);
            if (jobResponse.Job == null)
            {
                response.ErrorMessage = jobResponse.ErrorMessage;
                return response;
            }

            response = await _jobService.GetCurrentJobsOfZookeeperAsync(jobDto.ZookeeperId);

            return response;
        }

        public async Task<GetJobsResponse> FinishJobAsync(int jobId)
        {
            var response = new GetJobsResponse();

            if (jobId <= 0)
            {
                response.ErrorMessage = "Task with negative or zero id doesn't exist";
                return response;
            }

            var jobResponse = await _jobService.FinishJobAsync(jobId);
            if (jobResponse.Job == null)
            {
                response.ErrorMessage = jobResponse.ErrorMessage;
                return response;
            }

            response = await _jobService.GetCurrentJobsOfZookeeperAsync(jobResponse.Job.ZookeeperId);

            return response;
        }
    }
}
