using MassTransit;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.Infrastructure.MassTransit.Requests;
using MicroZoo.Infrastructure.Models.Animals;

namespace MicroZoo.AnimalsApi.Consumers
{
    public class UpdateAnimalConsumer : IConsumer<UpdateAnimalRequest>
    {
        private readonly IAnimalsApiService _service;

        public UpdateAnimalConsumer(IAnimalsApiService service)
        {
            _service = service;
        }
        public async Task Consume(ConsumeContext<UpdateAnimalRequest> context)
        {
            var id = context.Message.Id;

            var animalDto = context.Message.AnimalDto ?? throw new ArgumentNullException("Request does not contain data");

            var updatedAnimal = await _service.UpdateAnimalAsync(id, animalDto);

            await context.RespondAsync<Animal>(updatedAnimal);
        }
    }
}
