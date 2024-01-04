using MassTransit;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.Infrastructure.MassTransit.Requests;
using MicroZoo.Infrastructure.MassTransit.Responses;
using MicroZoo.Infrastructure.Models.Animals;

namespace AnimalsApi.Consumers
{
    public class AddAnimalConsumer : IConsumer<AddAnimalRequest>
    {
        private readonly IAnimalsApiService _service;

        public AddAnimalConsumer(IAnimalsApiService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<AddAnimalRequest> context)
        {
            var animal = context.Message.Animal;

            if (animal == null)
                throw new ArgumentNullException("Request does not contain data");

           await _service.AddAnimalAsync(animal);

           await context.RespondAsync<Animal>(animal);
        }
    }
}
