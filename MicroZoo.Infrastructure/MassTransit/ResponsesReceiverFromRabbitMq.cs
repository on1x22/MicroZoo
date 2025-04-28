using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using MicroZoo.Infrastructure.CorrelationIdGenerator;

namespace MicroZoo.Infrastructure.MassTransit
{
    /// <summary>
    /// Provide to send data to other microservices through RabbitMq
    /// </summary>
    public class ResponsesReceiverFromRabbitMq : IResponsesReceiverFromRabbitMq
    {
        private readonly IServiceProvider _provider;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ICorrelationIdGenerator _correlationIdGenerator;

        /// <summary>
        /// Initializes a new instance of the ResponsesReceiverFromRabbitMq class 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="publishEndpoint"></param>
        public ResponsesReceiverFromRabbitMq(IServiceProvider provider, 
                                             IPublishEndpoint publishEndpoint,
                                             ICorrelationIdGenerator correlationIdGenerator)
        {
            _provider = provider;
            _publishEndpoint = publishEndpoint;
            _correlationIdGenerator = correlationIdGenerator;
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

        public async Task<TOut> GetResponseFromRabbitTask_v2<TIn, TOut>(TIn request, 
            Uri rabbitMqUrl/*, string correlationId*/)
            where TIn : class
            where TOut : class
        {
            var clientFactory = _provider.GetRequiredService<IClientFactory>();

            var client = clientFactory.CreateRequestClient<TIn>(rabbitMqUrl);
            var response = await client.GetResponse<TOut>(request, x => x.UseExecute(context =>
            context.Headers.Set("X-Correlation-Id", _correlationIdGenerator.GetCorrelationId())));

            
            return response.Message;
        }
    }
}
