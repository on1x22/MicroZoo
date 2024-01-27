using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace MicroZoo.Infrastructure.MassTransit
{
    public class ResponsesReceiverFromRabbitMq : IResponsesReceiverFromRabbitMq
    {
        private readonly IServiceProvider _provider;
        public ResponsesReceiverFromRabbitMq(IServiceProvider provider)
        {
            _provider = provider;
        }
        public async Task<TOut> GetResponseFromRabbitTask<TIn, TOut>(TIn request, Uri rabbitMqUrl)
            where TIn : class
            where TOut : class
        {
            var clientFactory = _provider.GetRequiredService<IClientFactory>();

            var client = clientFactory.CreateRequestClient<TIn>(rabbitMqUrl);
            var response = await client.GetResponse<TOut>(request);
            return response.Message;
        }
    }
}
