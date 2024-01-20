using MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.ZookeepersApi.Services;

namespace MicroZoo.ZookeepersApi.Consumers
{
    public class CheckZokeepersWithSpecialityAreExistConsumer : 
        IConsumer<CheckZokeepersWithSpecialityAreExistRequest>
    {
        private readonly IZookeeperApiService _service;

        public CheckZokeepersWithSpecialityAreExistConsumer(IZookeeperApiService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<CheckZokeepersWithSpecialityAreExistRequest> context)
        {
            var response = await _service.CheckZokeepersWithSpecialityAreExistAsync(
                context.Message.CheckType, context.Message.ObjectId);

            response.OperationId = context.Message.OperationId;
            
            await context.RespondAsync<CheckZokeepersWithSpecialityAreExistResponse>(response);
        }
    }
}
