using MassTransit;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.AuthService.Services;
using MicroZoo.AuthService.Policies;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;

namespace MicroZoo.AnimalsApi.Consumers
{
    public class GetAllAnimalsConsumer : IConsumer<GetAllAnimalsRequest>
    {
        private readonly IAnimalsApiService _service;
        private readonly IAnimalsRequestReceivingService _receivingService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IConnectionService _connectionService;

        public GetAllAnimalsConsumer(IAnimalsApiService service,
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
        public async Task Consume(ConsumeContext<GetAllAnimalsRequest> context)
        {
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: context.Message.AccessToken,
                type: typeof(GetAllAnimalsConsumer),
                methodName: nameof(Consume),
                identityApiUrl: _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)
            {
                await context.RespondAsync(new GetAnimalsResponse
                {
                    ErrorCode = accessResult.ErrorCode,
                    ErrorMessage = accessResult.ErrorMessage!
                });
                return;
            }

            //var response = await _service.GetAllAnimalsAsync();
            var response = await _receivingService.GetAllAnimalsAsync();
            response.OperationId = context.Message.OperationId;
            
            await context.RespondAsync<GetAnimalsResponse>(response);
        }
    }
}
