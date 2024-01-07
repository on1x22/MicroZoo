using MassTransit;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.Infrastructure.MassTransit.Requests;
using MicroZoo.Infrastructure.MassTransit.Responses;

namespace MicroZoo.AnimalsApi.Consumers
{
    public class DeleteAnimalTypeConsumer : IConsumer<DeleteAnimalTypeRequest>
    {
        private readonly IAnimalsApiService _service;

        public DeleteAnimalTypeConsumer(IAnimalsApiService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<DeleteAnimalTypeRequest> context)
        {
            var id = context.Message.Id;

            var response = await _service.DeleteAnimalTypeAsync(id);
            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetAnimalTypeResponse>(response);
        }
    }
}
