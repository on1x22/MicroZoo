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

        public async Task<GetJobsResponse> GetJobsForTimeRangeAsync(int zookeeperId, DateTime startDateTime, DateTime finishDateTime)
        {
            var response = new GetJobsResponse();

            if(zookeeperId == 0)
                response.Jobs = await _repository.GetAllJobsForTimeRangeAsync(startDateTime, 
                    finishDateTime);
            
            if(zookeeperId > 0)
                response.Jobs = await _repository.GetZookeeperJobsForTimeRangeAsync(zookeeperId, 
                    startDateTime, finishDateTime);
            
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

            if (oldJob == null)
            {
                response.ErrorMessage = $"Task with id={jobId} not exist";
                return response;
            }

            if (oldJob.FinishTime != null)
            {
                response.ErrorMessage = "Changing a completed task is not allowed";
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

            if (finishedJob == null)
            {
                response.ErrorMessage = $"Task with id={jobId} not exist";
                return response;
            }

            if (finishedJob.FinishTime != null)
            {
                response.ErrorMessage = $"Task wint id={jobId} already completed";
                return response;
            }

            response.Job = await _repository.FinishJobAsync(jobId);

            if (response.Job == null)
                response.ErrorMessage = "Failed to complete task. Please check the entered data";

            return response;
        }
    }
}
