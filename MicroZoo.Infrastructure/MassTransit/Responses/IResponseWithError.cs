namespace MicroZoo.Infrastructure.MassTransit.Responses
{
    public interface IResponseWithError
    {
        ResponseError ResponseError { get; }
    }
}
