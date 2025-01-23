using MassTransit;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.AuthService.Policies;
using MicroZoo.AuthService.Services;
using MicroZoo.Infrastructure.Exceptions;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;

namespace MicroZoo.AnimalsApi.Consumers
{
    public class AddAnimalTypeConsumer : IConsumer<AddAnimalTypeRequest>
    {
        private readonly IAnimalsApiService _service;
        private readonly IAnimalTypesRequestReceivingService _receivingService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IConnectionService _connectionService;

        public AddAnimalTypeConsumer(IAnimalsApiService service,
            IAnimalTypesRequestReceivingService receivingService,
            IAuthorizationService authorizationService,
            IConnectionService connectionService)
        {
            _service = service;
            _receivingService = receivingService;
            _authorizationService = authorizationService;
            _connectionService = connectionService;
        }

        [PolicyValidation(Policy = "AnimalsApi.Create")]
        public async Task Consume(ConsumeContext<AddAnimalTypeRequest> context)
        {
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: context.Message.AccessToken,
                type: typeof(AddAnimalTypeConsumer),
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

            var animalTypeDto = context.Message.AnimalTypeDto;

            if (animalTypeDto == null)
                throw new BadRequestException("Request does not contain data");

            var response = await _receivingService.AddAnimalTypeAsync(animalTypeDto);

            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetAnimalTypeResponse>(response);
        }
    }
}
