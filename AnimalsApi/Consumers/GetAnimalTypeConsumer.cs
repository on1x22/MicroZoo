using MassTransit;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;

namespace MicroZoo.AnimalsApi.Consumers
{
    public class GetAnimalTypeConsumer : IConsumer<GetAnimalTypeRequest>
    {
        private readonly IAnimalsApiService _service;

        public GetAnimalTypeConsumer(IAnimalsApiService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<GetAnimalTypeRequest> context)
        {
            var response = await _service.GetAnimalTypeAsync(context.Message.Id);
            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetAnimalTypeResponse>(response);
        }
    }
}
