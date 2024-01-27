using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Jobs.Dto;

namespace MicroZoo.ZookeepersApi.Services
{
    public interface IRequestReceivingService
    {
        Task<GetJobsResponse> AddJobAsync(JobDto jobDto);
    }
}
