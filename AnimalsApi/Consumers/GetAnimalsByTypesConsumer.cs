using MicroZoo.AnimalsApi.Services;
using MassTransit;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;

namespace MicroZoo.AnimalsApi.Consumers
{
    public class GetAnimalsByTypesConsumer : IConsumer<GetAnimalsByTypesRequest>
    {
        private readonly IAnimalsApiService _service;
        private readonly IAnimalsRequestReceivingService _receivingService;

        public GetAnimalsByTypesConsumer(IAnimalsApiService service,
            IAnimalsRequestReceivingService receivingService)
        {
            _service = service;
            _receivingService = receivingService;
        }

        public async Task Consume(ConsumeContext<GetAnimalsByTypesRequest> context)
        {
            //var response = await _service.GetAnimalsByTypesAsync(context.Message.AnimalTypesIds);
            var response = await _receivingService.GetAnimalsByTypesAsync(context.Message.AnimalTypesIds);

            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetAnimalsResponse>(response);
        }
    }
}
