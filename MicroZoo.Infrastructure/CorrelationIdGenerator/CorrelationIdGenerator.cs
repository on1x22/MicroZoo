namespace MicroZoo.Infrastructure.CorrelationIdGenerator
{
    public class CorrelationIdGenerator : ICorrelationIdGenerator
    {
        private string _correlationId = Guid.NewGuid().ToString();

        public string GetCorrelationId() => _correlationId;
        
        public void SetCorrelationId(string correlationId) => _correlationId = correlationId;        
    }
}
