using MassTransit;
using MicroZoo.Infrastructure.Exceptions;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;

namespace MicroZoo.AnimalsApi.Consumers
{
    public class UpdateAnimalConsumer : IConsumer<UpdateAnimalRequest>
    {
        private readonly IAnimalsApiService _service;
        private readonly IAnimalsRequestReceivingService _receivingService;

        public UpdateAnimalConsumer(IAnimalsApiService service,
            IAnimalsRequestReceivingService receivingService)
        {
            _service = service;
            _receivingService = receivingService;
        }
        public async Task Consume(ConsumeContext<UpdateAnimalRequest> context)
        {
            var id = context.Message.Id;

            var animalDto = context.Message.AnimalDto ?? throw new BadRequestException("Request does not contain data");

            //var response = await _service.UpdateAnimalAsync(id, animalDto);
            var response = await _receivingService.UpdateAnimalAsync(id, animalDto);
            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetAnimalResponse>(response);
        }
    }
}
