
namespace MicroZoo.Infrastructure.MassTransit
{
    /// <summary>
    /// Provides to send data to other microservices through RabbitMq
    /// </summary>
    public interface IResponsesReceiverFromRabbitMq
    {
        /// <summary>
        /// Provides functionality of sending request to other microservices through RabbitMq
        /// </summary>
        /// <typeparam name="TIn">Type of requesting data</typeparam>
        /// <typeparam name="TOut">Type of connection string of other microservice</typeparam>
        /// <param name="request"></param>
        /// <param name="rabbitMqUrl"></param>
        /// <returns></returns>
        Task<TOut> GetResponseFromRabbitTask<TIn, TOut>(TIn request, Uri rabbitMqUrl) 
            where TIn : class
            where TOut : class;
    }
}
