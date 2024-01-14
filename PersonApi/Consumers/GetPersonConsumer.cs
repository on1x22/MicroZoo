using MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests;
using MicroZoo.Infrastructure.MassTransit.Responses;
using MicroZoo.PersonsApi.Services;

namespace MicroZoo.PersonsApi.Consumers
{
    public class GetPersonConsumer : IConsumer<GetPersonRequest>
    {
        private readonly IPersonsApiService _service;

        public GetPersonConsumer(IPersonsApiService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<GetPersonRequest> context)
        {
            var response = await _service.GetPersonAsync(context.Message.Id);
            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetPersonResponse>(response);
        }
    }
}
