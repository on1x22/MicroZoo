
namespace MicroZoo.Infrastructure.MassTransit
{
    public interface IResponsesReceiverFromRabbitMq
    {
        Task<TOut> GetResponseFromRabbitTask<TIn, TOut>(TIn request, Uri rabbitMqUrl) 
            where TIn : class
            where TOut : class;
    }
}
