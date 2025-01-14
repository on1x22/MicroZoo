
namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    public class FinishJobRequest
    {
        public Guid OperationId { get; set; }
        public int JobId { get; set; }
        public string JobReport { get; set; }
        public string AccessToken { get; }

        public FinishJobRequest(int jobId, string jobReport, string accessToken)
        {
            OperationId = Guid.NewGuid();
            JobId = jobId;
            JobReport = jobReport;
            AccessToken = accessToken;
        }
    }
}
