using MassTransit;
using MicroZoo.AuthService.Policies;
using MicroZoo.AuthService.Services;
using MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi;
using MicroZoo.PersonsApi.Services;

namespace MicroZoo.PersonsApi.Consumers
{
    /// <summary>
    /// Consumes messages from RabbitMq about getting person
    /// </summary>
    public class GetPersonConsumer : IConsumer<GetPersonRequest>
    {
        private readonly IPersonsApiService _personsService;
        private readonly IPersonsRequestReceivingService _receivingService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IConnectionService _connectionService;

        /// <summary>
        /// Initialize a new instance of <see cref="GetPersonConsumer"/> class 
        /// </summary>
        /// <param name="personsService"></param>
        /// <param name="receivingService"></param>
        /// <param name="authorizationService"></param>
        /// <param name="connectionService"></param>
        public GetPersonConsumer(IPersonsApiService personsService,
            IPersonsRequestReceivingService receivingService,
            IAuthorizationService authorizationService,
            IConnectionService connectionService)
        {
            _personsService = personsService;
            _receivingService = receivingService;
            _authorizationService = authorizationService;
            _connectionService = connectionService;
        }

        /// <summary>
        /// Consume messages from RabbitMq about getting person
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [PolicyValidation(Policy = "PersonsApi.Read")]
        public async Task Consume(ConsumeContext<GetPersonRequest> context)
        {
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: context.Message.AccessToken,
                type: typeof(GetPersonConsumer),
                methodName: nameof(Consume),
                identityApiUrl: _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)
                return /*accessResult.Result*/;

            var response = await _receivingService.GetPersonAsync(context.Message.Id);
            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetPersonResponse>(response);
        }
    }
}
