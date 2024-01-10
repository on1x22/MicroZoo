using MassTransit;
using MicroZoo.AnimalsApi.Services;
using microZoo.Infrastructure.Exceptions;
using MicroZoo.Infrastructure.MassTransit.Requests;
using MicroZoo.Infrastructure.MassTransit.Responses;

namespace MicroZoo.AnimalsApi.Consumers
{
    public class AddAnimalTypeConsumer : IConsumer<AddAnimalTypeRequest>
    {
        private readonly IAnimalsApiService _service;

        public AddAnimalTypeConsumer(IAnimalsApiService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<AddAnimalTypeRequest> context)
        {
            var animalTypeDto = context.Message.AnimalTypeDto;

            if (animalTypeDto == null)
                throw new BadRequestException("Request does not contain data");

            var response = await _service.AddAnimalTypeAsync(animalTypeDto);
            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetAnimalTypeResponse>(response);
        }
    }
}
