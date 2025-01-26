using MassTransit;
using MicroZoo.AuthService.Policies;
using MicroZoo.AuthService.Services;
using MicroZoo.Infrastructure.Exceptions;
using MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi;
using MicroZoo.PersonsApi.Services;

namespace MicroZoo.PersonsApi.Consumers
{
    /// <summary>
    /// Provides receive requests from RabbitMq to add person to database
    /// </summary>
    public class AddPersonConsumer : IConsumer<AddPersonRequest>
    {
        private readonly IPersonsApiService _service;
        private readonly IPersonsRequestReceivingService _receivingService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IConnectionService _connectionService;

        /// <summary>
        /// Initialize a new instance of <see cref="AddPersonConsumer"/> class
        /// </summary>
        /// <param name="service"></param>
        /// <param name="receivingService"></param>
        /// <param name="authorizationService"></param>
        /// <param name="connectionService"></param>
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

        /// <summary>
        /// Asynchronous processes requests from RabbitMq to add person to database
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
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
