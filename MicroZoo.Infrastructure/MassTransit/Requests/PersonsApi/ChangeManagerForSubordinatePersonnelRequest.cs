namespace MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi
{
    /// <summary>
    /// Allows to delete person by request receiving from RabbitMq
    /// </summary>
    public class ChangeManagerForSubordinatePersonnelRequest
    {
        /// <summary>
        /// Id of the request
        /// </summary>
        public Guid OperationId { get; set; }

        /// <summary>
        /// Id of the employee's current manager
        /// </summary>
        public int CurrentManagerId { get; set; }

        /// <summary>
        /// Id of the employee's new manager
        /// </summary>
        public int NewManagerId { get; set; }

        /// <summary>
        /// AccessToken for check authentication
        /// </summary>
        public string AccessToken { get; }

        /// <summary>
        /// Initialize a new instance of <see cref="ChangeManagerForSubordinatePersonnelRequest"/> 
        /// class
        /// </summary>
        /// <param name="currentManagerId"></param>
        /// <param name="newManagerId"></param>
        /// <param name="accessToken"></param>
        public ChangeManagerForSubordinatePersonnelRequest(int currentManagerId,
                                                           int newManagerId,
                                                           string accessToken)
        {
            OperationId = Guid.NewGuid();
            CurrentManagerId = currentManagerId;
            NewManagerId = newManagerId;
            AccessToken = accessToken;
        }
    }
}
