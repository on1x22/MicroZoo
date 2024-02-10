using MassTransit;
using MicroZoo.Infrastructure.Exceptions;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
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

            var animalDto = context.Message.AnimalDto ?? throw new BadRequestException("Request does not contain data");

            var response = await _service.UpdateAnimalAsync(id, animalDto);
            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetAnimalResponse>(response);
        }
    }
}
