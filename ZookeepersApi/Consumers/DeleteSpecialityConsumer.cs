using MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.ZookeepersApi.Services;

namespace ZookeepersApi.Consumers
{
    public class DeleteSpecialityConsumer : IConsumer<DeleteSpecialityRequest>
    {
        private readonly ISpecialitiesService _service;

        public DeleteSpecialityConsumer(ISpecialitiesService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<DeleteSpecialityRequest> context)
        {
            var response = await _service.DeleteSpecialityAsync(context.Message.SpecialityDto);

            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetSpecialitiesResponse>(response);
        }
    }
}
