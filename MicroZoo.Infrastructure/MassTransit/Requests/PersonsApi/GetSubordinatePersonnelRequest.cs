namespace MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi
{
    /// <summary>
    /// Allows to get information about subordinate personnel of person 
    /// by request receiving from RabbitMq
    /// </summary>
    public class GetSubordinatePersonnelRequest
    {
        /// <summary>
        /// Id of the request
        /// </summary>
        public Guid OperationId { get; set; }

        /// <summary>
        /// Id of person that managing of personnel
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// AccessToken for check authentication
        /// </summary>
        public string AccessToken { get; }

        /// <summary>
        /// Initialize a new instance of <see cref="GetSubordinatePersonnelRequest"/> class
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="accessToken"></param>
        public GetSubordinatePersonnelRequest(int personId, string accessToken)
        {
            OperationId = Guid.NewGuid();
            PersonId = personId;
            AccessToken = accessToken;
        }
    }
}
