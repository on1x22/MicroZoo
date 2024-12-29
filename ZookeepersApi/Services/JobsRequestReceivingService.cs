using MicroZoo.Infrastructure.Generals;
using MicroZoo.Infrastructure.MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Jobs.Dto;

namespace MicroZoo.ZookeepersApi.Services
{
    /// <summary>
    /// Procceses request from controllers and RabbitMq consumers. Interconnections with other 
    /// microservices doing here 
    /// </summary>
    public class JobsRequestReceivingService : IJobsRequestReceivingService
    {
        private readonly IJobsService _jobService;
        private readonly IResponsesReceiverFromRabbitMq _receiver;
        private readonly IConnectionService _connectionService;

        /// <summary>
        /// Initializes a new instance of <see cref="JobsRequestReceivingService"/> class 
        /// </summary>
        /// <param name="jobService"></param>
        /// <param name="receiver"></param>
        /// <param name="connectionService"></param>
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

            try
            {
                if (zookeeperId <= 0)                
                    throw new InvalidDataException("Zookeeper Id must be more than 0");

                if (dateTimeRange.StartDateTime >= dateTimeRange.FinishDateTime)                
                    throw new InvalidDataException("Start time more or equals finish time");
                                   
                if (!await IsZookeeperExist(zookeeperId)) 
                    throw new InvalidDataException($"Zookeeper with id={zookeeperId} doesn't exist");                   
            }
            catch (InvalidDataException ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }

            if (dateTimeRange.FinishDateTime == default)
                dateTimeRange.FinishDateTime = DateTime.MaxValue;

            response = await _jobService.GetJobsForDateTimeRangeAsync(zookeeperId, 
                dateTimeRange, orderingOptions, pageOptions);

            return response;
        }

        public async Task<GetJobsResponse> AddJobAsync(JobDto jobDto)
        {
            var response = new GetJobsResponse();

            try
            {
                if (jobDto.ZookeeperId <= 0)
                    throw new InvalidDataException("Zookeeper Id must be more than 0");

                if (jobDto.StartTime != default && jobDto.StartTime < DateTime.UtcNow)
                    throw new InvalidDataException("Start time less than current time");

                if (jobDto.DeadlineTime == default)
                    throw new InvalidDataException("Deadline didn't set");

                if (jobDto.DeadlineTime <= jobDto.StartTime)
                    throw new InvalidDataException("Deadline is less or equal start time");

                if (jobDto.Priority <= 0)
                    throw new InvalidDataException("Priority must be more than 0");

                if (!await IsZookeeperExist(jobDto.ZookeeperId))                
                    throw new InvalidDataException($"Zookeeper with id={jobDto.ZookeeperId} " +
                        $"doesn't exist");                  
            }
            catch (InvalidDataException ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }

            if (jobDto.StartTime == default)
                jobDto.StartTime = DateTime.UtcNow;

            var jobResponse = await _jobService.AddJobAsync(jobDto);
            
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

            try
            {
                if (jobId <= 0)                
                    throw new InvalidDataException("Task with negative or zero id doesn't exist");
                  
                if (jobDto.ZookeeperId <= 0)
                    throw new InvalidDataException("Zookeeper with negative or zero id doesn't " +
                        "exist");
                    
                if (jobDto.Description.Length < 10)
                    throw new InvalidDataException("Task description must consist of 10 or more " +
                        "symbols");

                if (jobDto.DeadlineTime == default)
                    throw new InvalidDataException("Deadline didn't set");

                if (jobDto.Priority <= 0)
                    throw new InvalidDataException("Priority must be more than 0");

                if (!await IsZookeeperExist(jobDto.ZookeeperId))
                    throw new InvalidDataException($"Zookeeper with id={jobDto.ZookeeperId} " +
                        $"doesn't exist");
                  
            }
            catch (InvalidDataException ex)
            {
                response.ErrorMessage = ex.Message;
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

        public async Task<GetJobsResponse> FinishJobAsync(int jobId, string jobReport)
        {
            var response = new GetJobsResponse();

            try
            {
                if (jobId <= 0)                
                    throw new InvalidDataException("Task with negative or zero id doesn't exist");    
                if (jobReport.Length == 0)
                    throw new InvalidDataException("Report must contain a comment line");
            }
            catch (InvalidDataException ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }

            var jobResponse = await _jobService.FinishJobAsync(jobId, jobReport);
            if (jobResponse.Job == null)
            {
                response.ErrorMessage = jobResponse.ErrorMessage;
                return response;
            }

            response = await _jobService.GetCurrentJobsOfZookeeperAsync(jobResponse.Job.ZookeeperId);

            return response;
        }

        private async Task<bool> IsZookeeperExist(int zookeeperId)
        {
            var personResponse = await _receiver.GetResponseFromRabbitTask<GetPersonRequest,
                    GetPersonResponse>(new GetPersonRequest(zookeeperId), _connectionService.PersonsApiUrl);

            if (personResponse.Person == null)
                return false;

            return personResponse.Person.IsManager != true;
        }
    }
}
