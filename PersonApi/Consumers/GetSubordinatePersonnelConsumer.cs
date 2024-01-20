using MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi;
using MicroZoo.PersonsApi.Services;

namespace MicroZoo.PersonsApi.Consumers
{
    public class GetSubordinatePersonnelConsumer : IConsumer<GetSubordinatePersonnelRequest>
    {
        private readonly IPersonsApiService _service;

        public GetSubordinatePersonnelConsumer(IPersonsApiService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<GetSubordinatePersonnelRequest> context)
        {
            var response = await _service.GetSubordinatePersonnelAsync(context.Message.PersonId);
            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetPersonsResponse>(response);
        }
    }
}
