using MassTransit;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.Infrastructure.MassTransit.Requests;
using MicroZoo.Infrastructure.MassTransit.Responses;

namespace MicroZoo.AnimalsApi.Consumers
{
    public class GetAllAnimalsConsumer : IConsumer<GetAllAnimalsRequest>
    {
        private readonly IAnimalsApiService _service;

        public GetAllAnimalsConsumer(IAnimalsApiService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<GetAllAnimalsRequest> context)
        {
            var response = await _service.GetAllAnimalsAsync();
            response.OperationId = context.Message.OperationId;
            
            await context.RespondAsync<GetAnimalsResponse>(response);
        }
    }
}
