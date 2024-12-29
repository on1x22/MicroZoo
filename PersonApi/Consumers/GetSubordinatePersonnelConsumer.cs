using MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi;
using MicroZoo.PersonsApi.Services;

namespace MicroZoo.PersonsApi.Consumers
{
    public class GetSubordinatePersonnelConsumer : IConsumer<GetSubordinatePersonnelRequest>
    {
        private readonly IPersonsApiService _service;
        private readonly IPersonsRequestReceivingService _receivingService;

        public GetSubordinatePersonnelConsumer(IPersonsApiService service,
            IPersonsRequestReceivingService receivingService)
        {
            _service = service;
            _receivingService = receivingService;
        }

        public async Task Consume(ConsumeContext<GetSubordinatePersonnelRequest> context)
        {
            //var response = await _service.GetSubordinatePersonnelAsync(context.Message.PersonId);
            var response = await _receivingService.GetSubordinatePersonnelAsync(context.Message.PersonId);
            
            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetPersonsResponse>(response);
        }
    }
}
