using MassTransit;
using MicroZoo.AuthService.Policies;
using MicroZoo.AuthService.Services;
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
        private readonly IAuthorizationService _authorizationService;
        private readonly IConnectionService _connectionService;

        public AddPersonConsumer(IPersonsApiService service, 
            IPersonsRequestReceivingService receivingService,
            IAuthorizationService authorizationService,
            IConnectionService connectionService)
        {
            _service = service;
            _receivingService = receivingService;
            _authorizationService = authorizationService;
            _connectionService = connectionService;
        }

        [PolicyValidation(Policy = "PersonsApi.Create")]
        public async Task Consume(ConsumeContext<AddPersonRequest> context)
        {
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: context.Message.AccessToken,
                type: typeof(AddPersonConsumer),
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

            var personDto = context.Message.PersonDto;

            if (personDto == null)
                throw new BadRequestException("Request does not contain data");

            var response = await _receivingService.AddPersonAsync(personDto);
            
            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetPersonResponse>(response);
        }
    }
}
