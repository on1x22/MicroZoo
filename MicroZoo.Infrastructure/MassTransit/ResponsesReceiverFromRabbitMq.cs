using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace MicroZoo.Infrastructure.MassTransit
{
    /// <summary>
    /// Provide to send data to other microservices through RabbitMq
    /// </summary>
    public class ResponsesReceiverFromRabbitMq : IResponsesReceiverFromRabbitMq
    {
        private readonly IServiceProvider _provider;

        /// <summary>
        /// Initializes a new instance of the ResponsesReceiverFromRabbitMq class 
        /// </summary>
        /// <param name="provider"></param>
        public ResponsesReceiverFromRabbitMq(IServiceProvider provider)
        {
            _provider = provider;
        }
        
        /// <summary>
        /// Provides functionality of sending request to other microservices through RabbitMq
        /// </summary>
        /// <typeparam name="TIn">Type of requesting data</typeparam>
        /// <typeparam name="TOut">Type of connection string of other microservice</typeparam>
        /// <param name="request"></param>
        /// <param name="rabbitMqUrl"></param>
        /// <returns></returns>
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
