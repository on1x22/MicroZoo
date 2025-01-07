using MassTransit;
using MicroZoo.Infrastructure.Exceptions;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.AnimalsApi.Services;

namespace MicroZoo.AnimalsApi.Consumers
{
    public class AddAnimalConsumer : IConsumer<AddAnimalRequest>
    {
        private readonly IAnimalsApiService _service;
        private readonly IAnimalsRequestReceivingService _receivingService;

        public AddAnimalConsumer(IAnimalsApiService service,
            IAnimalsRequestReceivingService receivingService)
        {
            _service = service;
            _receivingService = receivingService;
        }

        public async Task Consume(ConsumeContext<AddAnimalRequest> context)
        {
            var animalDto = context.Message.AnimalDto;

            if (animalDto == null)
                throw new BadRequestException("Request does not contain data");
            
            //var response = await _service.AddAnimalAsync(animalDto);
            var response = await _receivingService.AddAnimalAsync(animalDto);
            response.OperationId = context.Message.OperationId;
            
            await context.RespondAsync<GetAnimalResponse>(response);
        }
    }
}
