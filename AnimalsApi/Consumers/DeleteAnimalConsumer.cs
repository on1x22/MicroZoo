using MassTransit;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.Infrastructure.MassTransit.Requests;
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

            var animal = await _service.DeleteAnimalAsync(id);

            await context.RespondAsync<Animal>(animal);
        }
    }
}
