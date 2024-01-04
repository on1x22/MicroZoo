using Infrastructure.MassTransit.Requests;
using MassTransit;
using MicroZoo.AnimalsApi.Services;
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
            var animal = await _service.GetAnimalAsync(context.Message.Id);

            await context.RespondAsync<Animal>(animal);
        }
    }
}
