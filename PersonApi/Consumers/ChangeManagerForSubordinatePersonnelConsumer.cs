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
        private readonly IPersonsRequestReceivingService _receivingService;

        public ChangeManagerForSubordinatePersonnelConsumer(IPersonsApiService service,
            IPersonsRequestReceivingService receivingService)
        {
            _service = service;
            _receivingService = receivingService;
        }

        public async Task Consume(ConsumeContext<ChangeManagerForSubordinatePersonnelRequest> context)
        {
            //var response = await _service.ChangeManagerForSubordinatePersonnel(
            //    context.Message.CurrentManagerId, context.Message.NewManagerId);
            var response = await _receivingService.ChangeManagerForSubordinatePersonnel(
                context.Message.CurrentManagerId, context.Message.NewManagerId);

            response. OperationId = context.Message.OperationId;
            
            await context.RespondAsync<GetPersonsResponse>(response);
        }
    }
}
