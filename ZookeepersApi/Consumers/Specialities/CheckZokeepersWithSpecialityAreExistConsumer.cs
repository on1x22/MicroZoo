using MassTransit;
using MicroZoo.AuthService.Policies;
using MicroZoo.AuthService.Services;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.ZookeepersApi.Services;

namespace MicroZoo.ZookeepersApi.Consumers.Specialities
{
    public class CheckZokeepersWithSpecialityAreExistConsumer :
        IConsumer<CheckZokeepersWithSpecialityAreExistRequest>
    {
        private readonly ISpecialitiesService _service;
        private readonly IAuthorizationService _authorizationService;
        private readonly IConnectionService _connectionService;

        public CheckZokeepersWithSpecialityAreExistConsumer(ISpecialitiesService service,
            IAuthorizationService authorizationService, IConnectionService connectionService)
        {
            _service = service;
            _authorizationService = authorizationService;
            _connectionService = connectionService;
        }

        [PolicyValidation(Policy = "ZookeepersApi.Read")]
        public async Task Consume(ConsumeContext<CheckZokeepersWithSpecialityAreExistRequest> context)
        {
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: context.Message.AccessToken,
                type: typeof(CheckZokeepersWithSpecialityAreExistConsumer),
                methodName: nameof(Consume),
                identityApiUrl: _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)
            {
                await context.RespondAsync(new CheckZokeepersWithSpecialityAreExistResponse
                {
                    ActionResult = accessResult.Result
                });
                return;
            }

            var response = await _service.CheckZokeepersWithSpecialityAreExistAsync(
                context.Message.CheckType, context.Message.ObjectId);

            response.OperationId = context.Message.OperationId;

            await context.RespondAsync(response);
        }
    }
}
