
namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    public class FinishJobRequest
    {
        public Guid OperationId { get; set; }
        public int JobId { get; set; }
        public string JobReport { get; set; }

        public FinishJobRequest(int jobId, string jobReport)
        {
            OperationId = Guid.NewGuid();
            JobId = jobId;
            JobReport = jobReport;
        }
    }
}
