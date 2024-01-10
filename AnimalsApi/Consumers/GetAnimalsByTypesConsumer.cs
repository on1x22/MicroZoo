using MassTransit;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.Infrastructure.MassTransit.Requests;
using MicroZoo.Infrastructure.MassTransit.Responses;

namespace MicroZoo.AnimalsApi.Consumers
{
    public class GetAnimalsByTypesConsumer : IConsumer<GetAnimalsByTypesRequest>
    {
        private readonly IAnimalsApiService _service;

        public GetAnimalsByTypesConsumer(IAnimalsApiService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<GetAnimalsByTypesRequest> context)
        {
            var response = await _service.GetAnimalsByTypesAsync(context.Message.AnimalTypesIds);
            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetAnimalsResponse>(response);
        }
    }
}
