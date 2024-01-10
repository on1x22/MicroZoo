using MassTransit;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.Infrastructure.MassTransit.Requests;
using MicroZoo.Infrastructure.MassTransit.Responses;
using MicroZoo.Infrastructure.Models.Animals;

namespace AnimalsApi.Consumers
{
    public class GetAnimalConsumer : IConsumer<GetAnimalRequest>
    {
        private readonly IAnimalsApiService _service;

        public GetAnimalConsumer(IAnimalsApiService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<GetAnimalRequest> context)
        {
            var response = await _service.GetAnimalAsync(context.Message.Id);
            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetAnimalResponse>(response);
        }
    }
}
