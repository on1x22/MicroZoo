using MicroZoo.Infrastructure.Generals;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Jobs;
using MicroZoo.Infrastructure.Models.Jobs.Dto;
using MicroZoo.ZookeepersApi.Repository;

namespace MicroZoo.ZookeepersApi.Services
{
    public class JobsService : IJobsService
    {
        private readonly IJobsRepository _repository;
        private readonly ILogger _logger;

        public JobsService(IJobsRepository repository, ILogger<JobsService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<GetJobsResponse> GetAllJobsOfZookeeperAsync(int zookeeperId) =>    
            new GetJobsResponse()
            {
                Jobs = await _repository.GetAllJobsOfZookeeperAsync(zookeeperId)
            };

        public async Task<GetJobsResponse> GetCurrentJobsOfZookeeperAsync(int zookeeperId) =>
            new GetJobsResponse()
            {
                Jobs = await _repository.GetCurrentJobsOfZookeeperAsync(zookeeperId)
            };

        public async Task<GetJobsResponse> GetJobsForDateTimeRangeAsync(int zookeeperId,
            DateTimeRange dateTimeRange, OrderingOptions orderingOptions, PageOptions pageOptions)
        {
            var response = new GetJobsResponse();

            if(zookeeperId == 0)
                response.Jobs = await _repository.GetAllJobsForDateTimeRangeAsync(dateTimeRange, 
                    orderingOptions, pageOptions);
            
            if(zookeeperId > 0)
                response.Jobs = await _repository.GetZookeeperJobsForDateTimeRangeAsync(zookeeperId, 
                    dateTimeRange, orderingOptions, pageOptions);
            
            return response;
        }

        public async Task<GetJobResponse> AddJobAsync(JobDto jobDto)
        {
            var response = new GetJobResponse()
            {
                Job = await _repository.AddJobAsync(jobDto)
            };

            if (response.Job == null)            
                response.ErrorMessage = "Failed to create a new task. Please check the entered data";
             
            return response;
        }

        public async Task<GetJobResponse> UpdateJobAsync(int jobId, JobWithoutStartTimeDto jobDto)
        {
            var oldJob = await _repository.GetJobAsync(jobId);

            var response = new GetJobResponse();

            try
            {
                if (jobId <= 0)
                    throw new InvalidDataException("Task with negative or zero id doesn't exist");

                if (oldJob == null)                
                    throw new InvalidDataException($"Task with id={jobId} not exist");
                    
                if (oldJob.FinishTime != null)                
                    throw new InvalidDataException("Changing a completed task is not allowed");
                    
                if (jobDto.DeadlineTime <= oldJob.StartTime)                
                    throw new InvalidDataException("New deadline time is less than start time");
            }
            catch (InvalidDataException ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }

            response.Job = await _repository.UpdateJobAsync(jobId, jobDto);

            if (response.Job == null)
                response.ErrorMessage = "Failed to update task. Please check the entered data";

            return response;
        }

        public async Task<GetJobResponse> FinishJobAsync(int jobId)
        {
            var finishedJob = await _repository.GetJobAsync(jobId);

            var response = new GetJobResponse();

            /*if (finishedJob == null)
            {
                response.ErrorMessage = $"Task with id={jobId} not exist";
                return response;
            }

            if (finishedJob.FinishTime != null)
            {
                response.ErrorMessage = $"Task wint id={jobId} already completed";
                return response;
            }*/

            try
            {
                if (finishedJob == null)                
                    throw new InvalidDataException($"Task with id={jobId} not exist");
                    
                if (finishedJob.FinishTime != null)                
                    throw new InvalidDataException($"Task wint id={jobId} already completed");
            }
            catch (InvalidDataException ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }

            response.Job = await _repository.FinishJobAsync(jobId);

            if (response.Job == null)
                response.ErrorMessage = "Failed to complete task. Please check the entered data";

            return response;
        }
    }
}
