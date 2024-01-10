using MassTransit;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.Infrastructure.MassTransit.Requests;
using MicroZoo.Infrastructure.MassTransit.Responses;

namespace AnimalsApi.Consumers
{
    public class GetAnimalTypesByIdsConsumer : IConsumer<GetAnimalTypesByIdsRequest>
    {
        private readonly IAnimalsApiService _service;

        public GetAnimalTypesByIdsConsumer(IAnimalsApiService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<GetAnimalTypesByIdsRequest> context)
        {
            var response = await _service.GetAnimalTypesByIdsAsync(context.Message.AnimalTypesIds);
            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetAnimalTypesResponse>(response);
        }
    }
}
