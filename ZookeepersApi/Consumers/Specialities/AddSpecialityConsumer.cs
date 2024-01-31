using MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.ZookeepersApi.Services;

namespace MicroZoo.ZookeepersApi.Consumers.Specialities
{
    public class AddSpecialityConsumer : IConsumer<AddSpecialityRequest>
    {
        private readonly ISpecialitiesService _service;

        public AddSpecialityConsumer(ISpecialitiesService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<AddSpecialityRequest> context)
        {
            var response = await _service.AddSpecialityAsync(context.Message.SpecialityDto);
            response.OperationId = context.Message.OperationId;

            await context.RespondAsync(response);
        }
    }
}
