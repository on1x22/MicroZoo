using MicroZoo.AnimalsApi.Services;
using MassTransit;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;

namespace MicroZoo.AnimalsApi.Consumers
{
    public class GetAllAnimalsConsumer : IConsumer<GetAllAnimalsRequest>
    {
        private readonly IAnimalsApiService _service;
        private readonly IAnimalsRequestReceivingService _receivingService;

        public GetAllAnimalsConsumer(IAnimalsApiService service,
            IAnimalsRequestReceivingService receivingService)
        {
            _service = service;
            _receivingService = receivingService;
        }

        public async Task Consume(ConsumeContext<GetAllAnimalsRequest> context)
        {
            //var response = await _service.GetAllAnimalsAsync();
            var response = await _receivingService.GetAllAnimalsAsync();
            response.OperationId = context.Message.OperationId;
            
            await context.RespondAsync<GetAnimalsResponse>(response);
        }
    }
}
