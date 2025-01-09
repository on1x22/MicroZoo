using MassTransit;
using MicroZoo.Infrastructure.Exceptions;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
using MicroZoo.AuthService.Services;
using MicroZoo.AuthService.Policies;

namespace MicroZoo.AnimalsApi.Consumers
{
    public class UpdateAnimalConsumer : IConsumer<UpdateAnimalRequest>
    {
        private readonly IAnimalsApiService _service;
        private readonly IAnimalsRequestReceivingService _receivingService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IConnectionService _connectionService;

        public UpdateAnimalConsumer(IAnimalsApiService service,
            IAnimalsRequestReceivingService receivingService,
            IAuthorizationService authorizationService,
            IConnectionService connectionService)
        {
            _service = service;
            _receivingService = receivingService;
            _authorizationService = authorizationService;
            _connectionService = connectionService;
        }

        [PolicyValidation(Policy = "AnimalsApi.Update")]
        public async Task Consume(ConsumeContext<UpdateAnimalRequest> context)
        {
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: context.Message.AccessToken,
                type: typeof(UpdateAnimalConsumer),
                methodName: nameof(Consume),
                identityApiUrl: _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)
            {
                await context.RespondAsync(new GetAnimalResponse
                {
                    ErrorCode = accessResult.ErrorCode,
                    ErrorMessage = accessResult.ErrorMessage
                });
                return;
            }

            var id = context.Message.Id;

            var animalDto = context.Message.AnimalDto ?? throw new BadRequestException("Request does not contain data");

            //var response = await _service.UpdateAnimalAsync(id, animalDto);
            var response = await _receivingService.UpdateAnimalAsync(id, animalDto);
            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetAnimalResponse>(response);
        }
    }
}
