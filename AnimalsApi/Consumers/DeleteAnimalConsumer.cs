using MassTransit;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.Infrastructure.MassTransit.Requests;
using MicroZoo.Infrastructure.MassTransit.Responses;
using MicroZoo.Infrastructure.Models.Animals;

namespace MicroZoo.AnimalsApi.Consumers
{
    public class DeleteAnimalConsumer : IConsumer<DeleteAnimalRequest>
    {
        private readonly IAnimalsApiService _service;

        public DeleteAnimalConsumer(IAnimalsApiService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<DeleteAnimalRequest> context)
        {
            var id = context.Message.Id;

            var response = await _service.DeleteAnimalAsync(id);
            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetAnimalResponse>(response);
        }
    }
}
