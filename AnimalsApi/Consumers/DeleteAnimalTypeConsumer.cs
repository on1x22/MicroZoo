using MassTransit;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.AuthService.Policies;
using MicroZoo.AuthService.Services;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;

namespace MicroZoo.AnimalsApi.Consumers
{
    /// <summary>
    /// Provides receive requests from RabbitMq to delete animal type from database
    /// </summary>
    public class DeleteAnimalTypeConsumer : IConsumer<DeleteAnimalTypeRequest>
    {
        private readonly IAnimalsApiService _service;
        private readonly IAnimalTypesRequestReceivingService _receivingService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IConnectionService _connectionService;

        /// <summary>
        /// Initialize a new instance of <see cref="DeleteAnimalTypeConsumer"/> class
        /// </summary>
        /// <param name="service"></param>
        /// <param name="receivingService"></param>
        /// <param name="authorizationService"></param>
        /// <param name="connectionService"></param>
        public DeleteAnimalTypeConsumer(IAnimalsApiService service,
            IAnimalTypesRequestReceivingService receivingService,
            IAuthorizationService authorizationService,
            IConnectionService connectionService)
        {
            _service = service;
            _receivingService = receivingService;
            _authorizationService = authorizationService;
            _connectionService = connectionService;
        }

        /// <summary>
        /// Asynchronous processes requests from RabbitMq to delete animal type from database
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [PolicyValidation(Policy = "AnimalsApi.Delete")]
        public async Task Consume(ConsumeContext<DeleteAnimalTypeRequest> context)
        {
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: context.Message.AccessToken,
                type: typeof(DeleteAnimalTypeConsumer),
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

            var id = context.Message.Id;
            var accessToken = context.Message.AccessToken;

            var response = await _receivingService.DeleteAnimalTypeAsync(id, accessToken);

            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetAnimalTypeResponse>(response);
        }
    }
}
