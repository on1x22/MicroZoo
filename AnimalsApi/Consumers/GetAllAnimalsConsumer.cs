using MassTransit;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.Infrastructure.MassTransit.Requests;
using MicroZoo.Infrastructure.MassTransit.Responses;

namespace MicroZoo.AnimalsApi.Consumers
{
    public class GetAllAnimalsConsumer : IConsumer<GetAnimalsRequest>
    {
        private readonly IAnimalsApiService _service;

        public GetAllAnimalsConsumer(IAnimalsApiService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<GetAnimalsRequest> context)
        {
            var animals = await _service.GetAllAnimalsAsync();
            
            await context.RespondAsync<GetAllAnimalsResponse>(new
            {
                OperationId = context.Message.OperationId,
                Animals = animals
            });
        }
    }
}
