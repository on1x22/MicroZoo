using MassTransit;
using microZoo.Infrastructure.Exceptions;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
using MicroZoo.Infrastructure.Models.Animals;

namespace MicroZoo.AnimalsApi.Consumers
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
            var animalDto = context.Message.AnimalDto;

            if (animalDto == null)
                throw new BadRequestException("Request does not contain data");
            
            var response = await _service.AddAnimalAsync(animalDto);
            response.OperationId = context.Message.OperationId;
            
            await context.RespondAsync<GetAnimalResponse>(response);
        }
    }
}
