using MassTransit;
using MicroZoo.AnimalsApi.Services;
using microZoo.Infrastructure.Exceptions;
using MicroZoo.Infrastructure.MassTransit.Requests;
using MicroZoo.Infrastructure.MassTransit.Responses;

namespace MicroZoo.AnimalsApi.Consumers
{
    public class UpdateAnimalTypeConsumer : IConsumer<UpdateAnimalTypeRequest>
    {
        private readonly IAnimalsApiService _service;

        public UpdateAnimalTypeConsumer(IAnimalsApiService service)
        {
            _service = service;
        }
        public async Task Consume(ConsumeContext<UpdateAnimalTypeRequest> context)
        {
            var id = context.Message.Id;

            var animalTypeDto = context.Message.AnimalTypeDto ?? throw new BadRequestException("Request does not contain data");

            var response = await _service.UpdateAnimalTypeAsync(id, animalTypeDto);
            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetAnimalTypeResponse>(response);
        }
    }
}
