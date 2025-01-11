namespace MicroZoo.Infrastructure.MassTransit.Responses
{
    public class __ResponseError
    {
        public string ErrorMessage { get; }
        public ErrorCodes ErrorCode { get; }

        public __ResponseError(string errorMessage, ErrorCodes errorCode)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
        }
    }
}
