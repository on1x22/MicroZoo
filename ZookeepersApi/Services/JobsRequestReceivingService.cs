using MicroZoo.Infrastructure.Generals;
using MicroZoo.Infrastructure.MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Jobs.Dto;

namespace MicroZoo.ZookeepersApi.Services
{
    public class JobsRequestReceivingService : IJobsRequestReceivingService
    {
        private readonly IJobsService _jobService;
        private readonly IResponsesReceiverFromRabbitMq _receiver;
        private readonly IConnectionService _connectionService;

        public JobsRequestReceivingService(IJobsService jobService, IResponsesReceiverFromRabbitMq receiver,
            IConnectionService connectionService)
        {
            _jobService = jobService;
            _receiver = receiver;
            _connectionService = connectionService;
        }

        public async Task<GetJobsResponse> GetAllJobsOfZookeeperAsync(int zookeeperId)
        {
            return await _jobService.GetAllJobsOfZookeeperAsync(zookeeperId);
        }

        public async Task<GetJobsResponse> GetCurrentJobsOfZookeeperAsync(int zookeeperId)
        {
            return await _jobService.GetCurrentJobsOfZookeeperAsync(zookeeperId);
        }

        public async Task<GetJobsResponse> GetJobsForDateTimeRangeAsync(int zookeeperId,
            DateTimeRange dateTimeRange, OrderingOptions orderingOptions, PageOptions pageOptions)
        {
            var response = new GetJobsResponse();

            if (zookeeperId < 0)
            {
                response.ErrorMessage = "Zookeeper with negative id doesn't exist";
                return response;
            }

            if (dateTimeRange.FinishDateTime == default)
                dateTimeRange.FinishDateTime = DateTime.MaxValue;

            if (dateTimeRange.StartDateTime >= dateTimeRange.FinishDateTime)
            {
                response.ErrorMessage = "Start time more or equals finish time";
                return response;
            }

            if (zookeeperId > 0)
            {
                var personResponse = await _receiver.GetResponseFromRabbitTask<GetPersonRequest,
                    GetPersonResponse>(new GetPersonRequest(zookeeperId), _connectionService.PersonsApiUrl);

                if (personResponse.Person == null || personResponse.Person.IsManager == true)
                {
                    response.ErrorMessage = $"Zookeeper with id={zookeeperId} doesn't exist";
                    return response;
                }
            }

            response = await _jobService.GetJobsForDateTimeRangeAsync(zookeeperId, 
                dateTimeRange, orderingOptions, pageOptions);

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
                    GetPersonResponse>(new GetPersonRequest(jobDto.ZookeeperId), _connectionService.PersonsApiUrl);

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
