namespace MicroZoo.Infrastructure.MassTransit.Requests.IdentityApi
{
    /// <summary>
    /// Provides information about access of user to certain resource
    /// </summary>
    public class CheckAccessRequest
    {
        /// <summary>
        /// Id of the request
        /// </summary>
        public Guid OperationId { get; set; }
        
        /// <summary>
        /// Access token of the request
        /// </summary>
        public string? AccessToken { get; set; }
        
        /// <summary>
        /// List of policies that consist a request
        /// </summary>
        public List<string>? Policies { get; set; }

        /// <summary>
        /// Initialize a new instance of <see cref="CheckAccessRequest"/> class 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="policies"></param>
        public CheckAccessRequest(string accessToken, List<string> policies)
        {
            OperationId = Guid.NewGuid();
            AccessToken = accessToken;
            Policies = policies;
        }
    }
}
