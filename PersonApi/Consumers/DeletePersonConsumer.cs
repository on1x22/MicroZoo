using MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi;
using MicroZoo.PersonsApi.Services;

namespace MicroZoo.PersonsApi.Consumers
{
    public class DeletePersonConsumer : IConsumer<DeletePersonRequest>
    {
        private readonly IPersonsApiService _service;

        public DeletePersonConsumer(IPersonsApiService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<DeletePersonRequest> context)
        {
            var personId = context.Message.PersonId;

            var response = await _service.DeletePersonAsync(personId);
            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetPersonResponse>(response);
        }
    }
}
