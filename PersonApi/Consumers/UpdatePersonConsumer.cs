using MassTransit;
using microZoo.Infrastructure.Exceptions;
using MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi;
using MicroZoo.PersonsApi.Services;

namespace MicroZoo.PersonsApi.Consumers
{
    public class UpdatePersonConsumer : IConsumer<UpdatePersonRequest>
    {
        private readonly IPersonsApiService _service;

        public UpdatePersonConsumer(IPersonsApiService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<UpdatePersonRequest> context)
        {
            var id = context.Message.PersonId;

            var personDto = context.Message.PersonDto ?? throw new BadRequestException("Request does not contain data");

            var response = await _service.UpdatePersonAsync(id, personDto);
            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetPersonResponse>(response);
        }
    }
}
