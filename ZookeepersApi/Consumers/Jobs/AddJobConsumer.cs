using MassTransit;
using MicroZoo.AuthService.Policies;
using MicroZoo.AuthService.Services;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.ZookeepersApi.Services;

namespace MicroZoo.ZookeepersApi.Consumers.Jobs
{
    public class AddJobConsumer : IConsumer<AddJobRequest>
    {
        private readonly IJobsRequestReceivingService _receivingService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IConnectionService _connectionService;

        public AddJobConsumer(IJobsRequestReceivingService receivingService,
            IAuthorizationService authorizationService,
            IConnectionService connectionService)
        {
            _receivingService = receivingService;
            _authorizationService = authorizationService;
            _connectionService = connectionService;
        }

        [PolicyValidation(Policy = "ZookeepersApi.Create")]
        public async Task Consume(ConsumeContext<AddJobRequest> context)
        {
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: context.Message.AccessToken,
                type: typeof(AddJobConsumer),
                methodName: nameof(Consume),
                identityApiUrl: _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)
            {
                await context.RespondAsync(new GetJobsResponse
                {
                    ErrorCode = accessResult.ErrorCode,
                    ErrorMessage = accessResult.ErrorMessage!
                });
                return;
            }

            var response = await _receivingService.AddJobAsync(context.Message.JobDto, 
                                                      context.Message.AccessToken);
            response.OperationId = context.Message.OperationId;

            await context.RespondAsync(response);
        }
    }
}
