namespace MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi
{
    public class ChangeManagerForSubordinatePersonnelRequest
    {
        public Guid OperationId { get; set; }
        public int CurrentManagerId { get; set; }
        public int NewManagerId { get; set; }
        public string AccessToken { get; }

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
