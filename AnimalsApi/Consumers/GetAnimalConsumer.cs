using MicroZoo.AnimalsApi.Services;
using MassTransit;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
using MicroZoo.Infrastructure.Models.Animals;

namespace MicroZoo.AnimalsApi.Consumers
{
    public class GetAnimalConsumer : IConsumer<GetAnimalRequest>
    {
        private readonly IAnimalsApiService _service;
        private readonly IAnimalsRequestReceivingService _receivingService;

        public GetAnimalConsumer(IAnimalsApiService service,
            IAnimalsRequestReceivingService receivingService)
        {
            _service = service;
            _receivingService = receivingService;
        }

        public async Task Consume(ConsumeContext<GetAnimalRequest> context)
        {
            //var response = await _service.GetAnimalAsync(context.Message.Id);
            var response = await _receivingService.GetAnimalAsync(context.Message.Id);
            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetAnimalResponse>(response);
        }
    }
}
