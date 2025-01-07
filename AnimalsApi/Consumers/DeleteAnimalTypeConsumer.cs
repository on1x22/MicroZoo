using MassTransit;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;

namespace MicroZoo.AnimalsApi.Consumers
{
    public class DeleteAnimalTypeConsumer : IConsumer<DeleteAnimalTypeRequest>
    {
        private readonly IAnimalsApiService _service;
        private readonly IAnimalTypesRequestReceivingService _receivingService;

        public DeleteAnimalTypeConsumer(IAnimalsApiService service,
            IAnimalTypesRequestReceivingService receivingService)
        {
            _service = service;
            _receivingService = receivingService;
        }

        public async Task Consume(ConsumeContext<DeleteAnimalTypeRequest> context)
        {
            var id = context.Message.Id;
            var accessToken = context.Message.AccessToken;

            //var response = await _service.DeleteAnimalTypeAsync(id);
            var response = await _receivingService.DeleteAnimalTypeAsync(id, accessToken);

            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetAnimalTypeResponse>(response);
        }
    }
}
