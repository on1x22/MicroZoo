using MassTransit;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.Infrastructure.Exceptions;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;

namespace MicroZoo.AnimalsApi.Consumers
{
    public class AddAnimalTypeConsumer : IConsumer<AddAnimalTypeRequest>
    {
        private readonly IAnimalsApiService _service;
        private readonly IAnimalTypesRequestReceivingService _receivingService;

        public AddAnimalTypeConsumer(IAnimalsApiService service,
            IAnimalTypesRequestReceivingService receivingService)
        {
            _service = service;
            _receivingService = receivingService;
        }

        public async Task Consume(ConsumeContext<AddAnimalTypeRequest> context)
        {
            var animalTypeDto = context.Message.AnimalTypeDto;

            if (animalTypeDto == null)
                throw new BadRequestException("Request does not contain data");

            //var response = await _service.AddAnimalTypeAsync(animalTypeDto);
            var response = await _receivingService.AddAnimalTypeAsync(animalTypeDto);

            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetAnimalTypeResponse>(response);
        }
    }
}
