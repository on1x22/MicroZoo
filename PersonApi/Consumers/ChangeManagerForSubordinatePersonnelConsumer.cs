using MassTransit;
using MicroZoo.AuthService.Policies;
using MicroZoo.AuthService.Services;
using MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi;
using MicroZoo.PersonsApi.Services;

namespace MicroZoo.PersonsApi.Consumers
{
    public class ChangeManagerForSubordinatePersonnelConsumer : 
        IConsumer<ChangeManagerForSubordinatePersonnelRequest>
    {
        private readonly IPersonsApiService _service;
        private readonly IPersonsRequestReceivingService _receivingService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IConnectionService _connectionService;

        public ChangeManagerForSubordinatePersonnelConsumer(IPersonsApiService service,
            IPersonsRequestReceivingService receivingService,
            IAuthorizationService authorizationService,
            IConnectionService connectionService)
        {
            _service = service;
            _receivingService = receivingService;
            _authorizationService = authorizationService;
            _connectionService = connectionService;
        }

        [PolicyValidation(Policy = "PersonsApi.Update")]
        public async Task Consume(ConsumeContext<ChangeManagerForSubordinatePersonnelRequest> context)
        {
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: context.Message.AccessToken,
                type: typeof(ChangeManagerForSubordinatePersonnelConsumer),
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

            //var response = await _service.ChangeManagerForSubordinatePersonnel(
            //    context.Message.CurrentManagerId, context.Message.NewManagerId);
            var response = await _receivingService.ChangeManagerForSubordinatePersonnel(
                context.Message.CurrentManagerId, context.Message.NewManagerId);

            response. OperationId = context.Message.OperationId;
            
            await context.RespondAsync<GetPersonsResponse>(response);
        }
    }
}
