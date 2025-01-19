using MassTransit;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.AuthService.Policies;
using MicroZoo.AuthService.Services;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;

namespace MicroZoo.AnimalsApi.Consumers
{
    public class GetAnimalTypesByIdsConsumer : IConsumer<GetAnimalTypesByIdsRequest>
    {
        private readonly IAnimalsApiService _service;
        private readonly IAnimalTypesRequestReceivingService _receivingService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IConnectionService _connectionService;

        public GetAnimalTypesByIdsConsumer(IAnimalsApiService service,
            IAnimalTypesRequestReceivingService receivingService,
            IAuthorizationService authorizationService,
            IConnectionService connectionService)
        {
            _service = service;
            _receivingService = receivingService;
            _authorizationService = authorizationService;
            _connectionService = connectionService;
        }

        [PolicyValidation(Policy = "AnimalsApi.Read")]
        public async Task Consume(ConsumeContext<GetAnimalTypesByIdsRequest> context)
        {
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: context.Message.AccessToken,
                type: typeof(GetAnimalTypesByIdsConsumer),
                methodName: nameof(Consume),
                identityApiUrl: _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)
            {
                await context.RespondAsync(new GetAnimalTypeResponse
                {
                    ErrorCode = accessResult.ErrorCode,
                    ErrorMessage = accessResult.ErrorMessage!
                });
                return;
            }

            //var response = await _service.GetAnimalTypesByIdsAsync(context.Message.AnimalTypesIds);
            var response = await _receivingService.GetAnimalTypesByIdsAsync(
                context.Message.AnimalTypesIds);

            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetAnimalTypesResponse>(response);
        }
    }
}
