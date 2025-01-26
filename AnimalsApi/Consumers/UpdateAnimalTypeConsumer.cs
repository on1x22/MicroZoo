using MassTransit;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.AuthService.Policies;
using MicroZoo.AuthService.Services;
using MicroZoo.Infrastructure.Exceptions;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;

namespace MicroZoo.AnimalsApi.Consumers
{
    /// <summary>
    /// Provides receive requests from RabbitMq to update animal type data in database
    /// </summary>
    public class UpdateAnimalTypeConsumer : IConsumer<UpdateAnimalTypeRequest>
    {
        private readonly IAnimalsApiService _service;
        private readonly IAnimalTypesRequestReceivingService _receivingService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IConnectionService _connectionService;

        /// <summary>
        /// Initialize a new instance of <see cref="UpdateAnimalTypeConsumer"/> class
        /// </summary>
        /// <param name="service"></param>
        /// <param name="receivingService"></param>
        /// <param name="authorizationService"></param>
        /// <param name="connectionService"></param>
        public UpdateAnimalTypeConsumer(IAnimalsApiService service,
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
        /// Asynchronous processes requests from RabbitMq to update animal type data in database
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        [PolicyValidation(Policy = "AnimalsApi.Update")]
        public async Task Consume(ConsumeContext<UpdateAnimalTypeRequest> context)
        {
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: context.Message.AccessToken,
                type: typeof(UpdateAnimalTypeConsumer),
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

            var animalTypeDto = context.Message.AnimalTypeDto ?? throw new BadRequestException("Request does not contain data");

            var response = await _receivingService.UpdateAnimalTypeAsync(id, animalTypeDto);

            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetAnimalTypeResponse>(response);
        }
    }
}
