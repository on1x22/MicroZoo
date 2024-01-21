using MassTransit;
using MicroZoo.Infrastructure.Exceptions;
using MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi;
using MicroZoo.PersonsApi.Services;

namespace MicroZoo.PersonsApi.Consumers
{
    public class ChangeManagerForSubordinatePersonnelConsumer : 
        IConsumer<ChangeManagerForSubordinatePersonnelRequest>
    {
        private readonly IPersonsApiService _service;

        public ChangeManagerForSubordinatePersonnelConsumer(IPersonsApiService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<ChangeManagerForSubordinatePersonnelRequest> context)
        {
            var response = await _service.ChangeManagerForSubordinatePersonnel(
                context.Message.CurrentManagerId, context.Message.NewManagerId);

            response. OperationId = context.Message.OperationId;
            
            await context.RespondAsync<GetPersonsResponse>(response);
        }
    }
}
