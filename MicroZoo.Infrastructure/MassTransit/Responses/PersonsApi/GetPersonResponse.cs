using MicroZoo.Infrastructure.Models.Persons;

namespace MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi
{
    /// <summary>
    /// Provides information about the response to a request for 
    /// information about a person
    /// </summary>
    public record GetPersonResponse : IResponseWithError
    {
        /// <summary>
        /// Id of the request
        /// </summary>
        public Guid OperationId { get; set; }

        /// <summary>
        /// Information about person
        /// </summary>
        public Person? Person { get; set; }

        /// <summary>
        /// Message about the occurred error 
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Status code of the occurred error 
        /// </summary>
        public ErrorCodes? ErrorCode { get; set; }
    }
}
