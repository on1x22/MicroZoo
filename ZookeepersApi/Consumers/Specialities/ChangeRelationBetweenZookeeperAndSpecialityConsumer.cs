using MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.ZookeepersApi.Services;

namespace MicroZoo.ZookeepersApi.Consumers.Specialities
{
    public class ChangeRelationBetweenZookeeperAndSpecialityConsumer :
        IConsumer<ChangeRelationBetweenZookeeperAndSpecialityRequest>
    {
        private readonly ISpecialitiesService _service;

        public ChangeRelationBetweenZookeeperAndSpecialityConsumer(ISpecialitiesService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<ChangeRelationBetweenZookeeperAndSpecialityRequest>
            context)
        {
            var response = await _service.ChangeRelationBetweenZookeeperAndSpecialityAsync(
                context.Message.RelationId, context.Message.SpecialityDto);
            response.OperationId = context.Message.OperationId;

            await context.RespondAsync(response);
        }
    }
}
