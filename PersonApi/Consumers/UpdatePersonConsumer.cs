using MassTransit;
using MicroZoo.Infrastructure.Exceptions;
using MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi;
using MicroZoo.PersonsApi.Services;

namespace MicroZoo.PersonsApi.Consumers
{
    public class UpdatePersonConsumer : IConsumer<UpdatePersonRequest>
    {
        private readonly IPersonsApiService _service;
        private readonly IPersonsRequestReceivingService _receivingService;

        public UpdatePersonConsumer(IPersonsApiService service,
            IPersonsRequestReceivingService receivingService)
        {
            _service = service;
            _receivingService = receivingService;
        }

        public async Task Consume(ConsumeContext<UpdatePersonRequest> context)
        {
            var id = context.Message.PersonId;

            var personDto = context.Message.PersonDto ?? throw new BadRequestException("Request does not contain data");

            //var response = await _service.UpdatePersonAsync(id, personDto);
            var response = await _receivingService.UpdatePersonAsync(id, personDto);
            
            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetPersonResponse>(response);
        }
    }
}
