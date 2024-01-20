namespace MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi
{
    public class DeletePersonRequest
    {
        public Guid OperationId { get; set; }
        public int PersonId { get; set; }

        public DeletePersonRequest(int personId)
        {
            OperationId = Guid.NewGuid();
            PersonId = personId;
        }
    }
}
