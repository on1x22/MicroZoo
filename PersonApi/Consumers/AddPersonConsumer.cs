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
        private readonly IPersonsRequestReceivingService _receivingService;

        public AddPersonConsumer(IPersonsApiService service, 
            IPersonsRequestReceivingService receivingService)
        {
            _service = service;
            _receivingService = receivingService;
        }

        public async Task Consume(ConsumeContext<AddPersonRequest> context)
        {
            var personDto = context.Message.PersonDto;

            if (personDto == null)
                throw new BadRequestException("Request does not contain data");

            //var response = await _service.AddPersonAsync(personDto);
            var response = await _receivingService.AddPersonAsync(personDto);
            
            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetPersonResponse>(response);
        }
    }
}
