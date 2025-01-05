using MassTransit;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.Infrastructure.Exceptions;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;

namespace MicroZoo.AnimalsApi.Consumers
{
    public class UpdateAnimalTypeConsumer : IConsumer<UpdateAnimalTypeRequest>
    {
        private readonly IAnimalsApiService _service;
        private readonly IAnimalTypesRequestReceivingService _receivingService;

        public UpdateAnimalTypeConsumer(IAnimalsApiService service,
            IAnimalTypesRequestReceivingService receivingService)
        {
            _service = service;
            _receivingService = receivingService;
        }
        public async Task Consume(ConsumeContext<UpdateAnimalTypeRequest> context)
        {
            var id = context.Message.Id;

            var animalTypeDto = context.Message.AnimalTypeDto ?? throw new BadRequestException("Request does not contain data");

            //var response = await _service.UpdateAnimalTypeAsync(id, animalTypeDto);
            var response = await _receivingService.UpdateAnimalTypeAsync(id, animalTypeDto);

            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetAnimalTypeResponse>(response);
        }
    }
}
