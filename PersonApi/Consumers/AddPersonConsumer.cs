using MassTransit;
using MicroZoo.Infrastructure.Exceptions;
using MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi;
using MicroZoo.PersonsApi.Services;

namespace MicroZoo.PersonsApi.Consumers
{
    public class AddPersonConsumer : IConsumer<AddPersonRequest>
    {
        private readonly IPersonsApiService _service;

        public AddPersonConsumer(IPersonsApiService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<AddPersonRequest> context)
        {
            var personDto = context.Message.PersonDto;

            if (personDto == null)
                throw new BadRequestException("Request does not contain data");

            var response = await _service.AddPersonAsync(personDto);
            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetPersonResponse>(response);
        }
    }
}
