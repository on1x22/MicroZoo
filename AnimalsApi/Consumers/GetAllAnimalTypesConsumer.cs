﻿using MassTransit;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.AuthService.Policies;
using MicroZoo.AuthService.Services;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;

namespace MicroZoo.AnimalsApi.Consumers
{
    /// <summary>
    /// Provides receive requests from RabbitMq to return all animal types from database
    /// </summary>
    public class GetAllAnimalTypesConsumer : IConsumer<GetAllAnimalTypesRequest>
    {
        private readonly IAnimalsApiService _service;
        private readonly IAnimalTypesRequestReceivingService _receivingService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IConnectionService _connectionService;

        /// <summary>
        /// Initialize a new instance of <see cref="GetAllAnimalTypesConsumer"/> class
        /// </summary>
        /// <param name="service"></param>
        /// <param name="receivingService"></param>
        /// <param name="authorizationService"></param>
        /// <param name="connectionService"></param>
        public GetAllAnimalTypesConsumer(IAnimalsApiService service,
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
        /// Asynchronous processes requests from RabbitMq to return all animal types from database
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [PolicyValidation(Policy = "AnimalsApi.Read")]
        public async Task Consume(ConsumeContext<GetAllAnimalTypesRequest> context)
        {
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: context.Message.AccessToken,
                type: typeof(GetAllAnimalTypesConsumer),
                methodName: nameof(Consume),
                identityApiUrl: _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)
            {
                await context.RespondAsync(new GetAnimalTypesResponse
                {
                    ErrorCode = accessResult.ErrorCode,
                    ErrorMessage = accessResult.ErrorMessage!
                });
                return;
            }

            var response = await _receivingService.GetAllAnimalTypesAsync();

            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetAnimalTypesResponse>(response);
        }
    }
}
