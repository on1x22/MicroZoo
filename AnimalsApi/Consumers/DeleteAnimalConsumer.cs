using MicroZoo.AnimalsApi.Services;
using MassTransit;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
using MicroZoo.Infrastructure.Models.Animals;

namespace MicroZoo.AnimalsApi.Consumers
{
    public class DeleteAnimalConsumer : IConsumer<DeleteAnimalRequest>
    {
        private readonly IAnimalsApiService _service;
        private readonly IAnimalsRequestReceivingService _receivingService;

        public DeleteAnimalConsumer(IAnimalsApiService service,
            IAnimalsRequestReceivingService receivingService)
        {
            _service = service;
            _receivingService = receivingService;
        }

        public async Task Consume(ConsumeContext<DeleteAnimalRequest> context)
        {
            var id = context.Message.Id;

            //var response = await _service.DeleteAnimalAsync(id);
            var response = await _receivingService.DeleteAnimalAsync(id);
            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetAnimalResponse>(response);
        }
    }
}
