using MassTransit;
using MicroZoo.AuthService.Policies;
using MicroZoo.AuthService.Services;
using MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi;
using MicroZoo.PersonsApi.Services;

namespace MicroZoo.PersonsApi.Consumers
{
    public class GetSubordinatePersonnelConsumer : IConsumer<GetSubordinatePersonnelRequest>
    {
        private readonly IPersonsApiService _service;
        private readonly IPersonsRequestReceivingService _receivingService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IConnectionService _connectionService;

        public GetSubordinatePersonnelConsumer(IPersonsApiService service,
            IPersonsRequestReceivingService receivingService,
            IAuthorizationService authorizationService,
            IConnectionService connectionService)
        {
            _service = service;
            _receivingService = receivingService;
            _authorizationService = authorizationService;
            _connectionService = connectionService;
        }

        [PolicyValidation(Policy = "PersonsApi.Read")]
        public async Task Consume(ConsumeContext<GetSubordinatePersonnelRequest> context)
        {
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: context.Message.AccessToken,
                type: typeof(GetSubordinatePersonnelConsumer),
                methodName: nameof(Consume),
                identityApiUrl: _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)
            {
                await context.RespondAsync(new GetPersonsResponse
                {
                    ErrorCode = accessResult.ErrorCode,
                    ErrorMessage = accessResult.ErrorMessage!
                });
                return;
            }

            //var response = await _service.GetSubordinatePersonnelAsync(context.Message.PersonId);
            var response = await _receivingService.GetSubordinatePersonnelAsync(context.Message.PersonId);
            
            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetPersonsResponse>(response);
        }
    }
}
