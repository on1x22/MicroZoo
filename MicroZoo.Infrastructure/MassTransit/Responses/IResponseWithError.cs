namespace MicroZoo.Infrastructure.MassTransit.Responses
{
    /// <summary>
    /// Provides information about errors in response
    /// </summary>
    public interface IResponseWithError
    {   
        /// <summary>
        /// Message describing error
        /// </summary>
        public string? ErrorMessage { get; }

        /// <summary>
        /// Http code of occurred error
        /// </summary>
        public ErrorCodes? ErrorCode { get; }
    }
}
