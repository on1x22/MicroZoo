using MassTransit;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;

namespace MicroZoo.AnimalsApi.Consumers
{
    public class GetAnimalTypesByIdsConsumer : IConsumer<GetAnimalTypesByIdsRequest>
    {
        private readonly IAnimalsApiService _service;
        private readonly IAnimalTypesRequestReceivingService _receivingService;

        public GetAnimalTypesByIdsConsumer(IAnimalsApiService service,
            IAnimalTypesRequestReceivingService receivingService)
        {
            _service = service;
            _receivingService = receivingService;
        }

        public async Task Consume(ConsumeContext<GetAnimalTypesByIdsRequest> context)
        {
            //var response = await _service.GetAnimalTypesByIdsAsync(context.Message.AnimalTypesIds);
            var response = await _receivingService.GetAnimalTypesByIdsAsync(
                context.Message.AnimalTypesIds);

            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetAnimalTypesResponse>(response);
        }
    }
}
