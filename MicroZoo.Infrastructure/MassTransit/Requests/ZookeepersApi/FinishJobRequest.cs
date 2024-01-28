
namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    public class FinishJobRequest
    {
        public Guid OperationId { get; set; }
        public int JobId { get; set; }

        public FinishJobRequest(int jobId)
        {
            OperationId = Guid.NewGuid();
            JobId = jobId;
        }
    }
}
