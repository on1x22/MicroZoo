using MicroZoo.AnimalsApi.Services;
using MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
using MicroZoo.AuthService.Services;
using MicroZoo.AuthService.Policies;

namespace MicroZoo.AnimalsApi.Consumers
{
    public class GetAnimalsByTypesConsumer : IConsumer<GetAnimalsByTypesRequest>
    {
        private readonly IAnimalsApiService _service;
        private readonly IAnimalsRequestReceivingService _receivingService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IConnectionService _connectionService;

        public GetAnimalsByTypesConsumer(IAnimalsApiService service,
            IAnimalsRequestReceivingService receivingService,
            IAuthorizationService authorizationService,
            IConnectionService connectionService)
        {
            _service = service;
            _receivingService = receivingService;
            _authorizationService = authorizationService;
            _connectionService = connectionService;
        }

        [PolicyValidation(Policy = "AnimalsApi.Read")]
        public async Task Consume(ConsumeContext<GetAnimalsByTypesRequest> context)
        {
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: context.Message.AccessToken,
                type: typeof(GetAnimalsByTypesConsumer),
                methodName: nameof(Consume),
                identityApiUrl: _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)
            {
                await context.RespondAsync(new GetAnimalsResponse
                {
                    ErrorCode = accessResult.ErrorCode,
                    ErrorMessage = accessResult.ErrorMessage
                });
                return;
            }

            //var response = await _service.GetAnimalsByTypesAsync(context.Message.AnimalTypesIds);
            var response = await _receivingService.GetAnimalsByTypesAsync(context.Message.AnimalTypesIds);

            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetAnimalsResponse>(response);
        }
    }
}
