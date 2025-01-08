namespace MicroZoo.Infrastructure.MassTransit.Responses
{
    public interface IResponseWithError
    {
        //ResponseError ResponseError { get; }
        public string? ErrorMessage { get; }
        public ErrorCodes? ErrorCode { get; }
    }
}
