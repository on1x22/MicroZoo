using MassTransit;
using MicroZoo.AuthService.Policies;
using MicroZoo.AuthService.Services;
using MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi;
using MicroZoo.PersonsApi.Services;

namespace MicroZoo.PersonsApi.Consumers
{
    public class DeletePersonConsumer : IConsumer<DeletePersonRequest>
    {
        //private readonly IPersonsApiService _service;
        private readonly IPersonsRequestReceivingService _receivingService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IConnectionService _connectionService;

        public DeletePersonConsumer(//IPersonsApiService service,
            IPersonsRequestReceivingService receivingService,
            IAuthorizationService authorizationService,
            IConnectionService connectionService)
        {
            //_service = service;
            _receivingService = receivingService;
            _authorizationService = authorizationService;
            _connectionService = connectionService;
        }

        [PolicyValidation(Policy = "PersonsApi.Delete")]
        public async Task Consume(ConsumeContext<DeletePersonRequest> context)
        {
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: context.Message.AccessToken,
                type: typeof(DeletePersonConsumer),
                methodName: nameof(Consume),
                identityApiUrl: _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)
            {
                await context.RespondAsync(new GetPersonResponse
                {
                    //ActionResult = accessResult.Result
                    ErrorCode = accessResult.ErrorCode,
                    ErrorMessage = accessResult.ErrorMessage
                });
                return;
            }
                
            var personId = context.Message.PersonId;

            var response = await _receivingService.DeletePersonAsync(personId, 
                context.Message.AccessToken);
            
            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetPersonResponse>(response);
        }
    }
}
