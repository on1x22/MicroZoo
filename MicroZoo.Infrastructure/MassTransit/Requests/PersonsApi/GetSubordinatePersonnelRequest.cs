namespace MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi
{
    public class GetSubordinatePersonnelRequest
    {
        public Guid OperationId { get; set; }
        public int PersonId { get; set; }
        public string AccessToken { get; }

        public GetSubordinatePersonnelRequest(int personId, string accessToken)
        {
            OperationId = Guid.NewGuid();
            PersonId = personId;
            AccessToken = accessToken;
        }
    }
}
