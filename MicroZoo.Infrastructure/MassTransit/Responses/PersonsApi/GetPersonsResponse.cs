using MicroZoo.Infrastructure.Models.Persons;

namespace MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi
{
    /// <summary>
    /// Provides information about the response to a request for 
    /// information about list of persons
    /// </summary>
    public record GetPersonsResponse : IResponseWithError
    {
        /// <summary>
        /// Id of the request
        /// </summary>
        public Guid OperationId { get; set; }

        /// <summary>
        /// Information about persons
        /// </summary>
        public List<Person>? Persons { get; set; }

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
