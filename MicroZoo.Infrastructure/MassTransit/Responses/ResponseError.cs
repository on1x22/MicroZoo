namespace MicroZoo.Infrastructure.MassTransit.Responses
{
    public class ResponseError
    {
        public string ErrorMessage { get; }
        public ErrorCodes ErrorCode { get; }

        public ResponseError(string errorMessage, ErrorCodes errorCode)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
        }
    }
}
