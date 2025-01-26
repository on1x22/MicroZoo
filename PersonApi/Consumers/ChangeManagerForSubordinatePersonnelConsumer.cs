using MassTransit;
using MicroZoo.AuthService.Policies;
using MicroZoo.AuthService.Services;
using MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi;
using MicroZoo.PersonsApi.Services;

namespace MicroZoo.PersonsApi.Consumers
{
    /// <summary>
    /// Provides receive requests from RabbitMq to change manager id for personnel in database
    /// </summary>
    public class ChangeManagerForSubordinatePersonnelConsumer : 
        IConsumer<ChangeManagerForSubordinatePersonnelRequest>
    {
        private readonly IPersonsApiService _service;
        private readonly IPersonsRequestReceivingService _receivingService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IConnectionService _connectionService;

        /// <summary>
        /// Initialize a new instance of 
        /// <see cref="ChangeManagerForSubordinatePersonnelConsumer"/> class
        /// </summary>
        /// <param name="service"></param>
        /// <param name="receivingService"></param>
        /// <param name="authorizationService"></param>
        /// <param name="connectionService"></param>
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

        /// <summary>
        /// Asynchronous processes requests from RabbitMq to change manager id 
        /// for personnel in database
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
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
