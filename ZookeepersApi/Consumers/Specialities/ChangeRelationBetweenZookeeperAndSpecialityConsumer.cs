using MassTransit;
using MicroZoo.AuthService.Policies;
using MicroZoo.AuthService.Services;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.ZookeepersApi.Services;

namespace MicroZoo.ZookeepersApi.Consumers.Specialities
{
    public class ChangeRelationBetweenZookeeperAndSpecialityConsumer :
        IConsumer<ChangeRelationBetweenZookeeperAndSpecialityRequest>
    {
        private readonly ISpecialitiesService _receivingService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IConnectionService _connectionService;

        public ChangeRelationBetweenZookeeperAndSpecialityConsumer(
            ISpecialitiesService receivingService,
            IAuthorizationService authorizationService,
            IConnectionService connectionService)
        {
            _receivingService = receivingService;
            _authorizationService = authorizationService;
            _connectionService = connectionService;
        }

        [PolicyValidation(Policy = "ZookeepersApi.Update")]
        public async Task Consume(ConsumeContext<ChangeRelationBetweenZookeeperAndSpecialityRequest>
            context)
        {
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: context.Message.AccessToken,
                type: typeof(ChangeRelationBetweenZookeeperAndSpecialityConsumer),
                methodName: nameof(Consume),
                identityApiUrl: _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)
            {
                await context.RespondAsync(new GetSpecialityResponse
                {
                    ErrorCode = accessResult.ErrorCode,
                    ErrorMessage = accessResult.ErrorMessage!
                });
                return;
            }

            var response = await _receivingService.ChangeRelationBetweenZookeeperAndSpecialityAsync(
                context.Message.RelationId, context.Message.SpecialityDto);
            response.OperationId = context.Message.OperationId;

            await context.RespondAsync(response);
        }
    }
}
