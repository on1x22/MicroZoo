using MicroZoo.Infrastructure.MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Jobs.Dto;

namespace MicroZoo.ZookeepersApi.Services
{
    public class RequestReceivingService : IRequestReceivingService
    {
        private readonly IJobsService _jobService;
        private readonly IResponsesReceiverFromRabbitMq _receiver;
        private readonly Uri _animalsApiUrl;
        private readonly Uri _personsApiUrl;
        private readonly Uri _zookeepersApiUrl;

        public RequestReceivingService(IJobsService jobService, IResponsesReceiverFromRabbitMq receiver,
            IConfiguration configuration)
        {
            _jobService = jobService;
            _receiver = receiver;
            _animalsApiUrl = new Uri(configuration["ConnectionStrings:AnimalsApiRmq"]);
            _personsApiUrl = new Uri(configuration["ConnectionStrings:PersonsApiRmq"]);
            _zookeepersApiUrl = new Uri(configuration["ConnectionStrings:ZookeepersApiRmq"]);
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
    }
}
