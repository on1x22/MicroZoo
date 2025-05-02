namespace MicroZoo.Gateway.CorrelationIdGenerator
{
    public class __CorrelationIdGenerator : __ICorrelationIdGenerator
    {
        private string _correlationId = Guid.NewGuid().ToString();

        public string GetCorrelationId() => _correlationId;
        
        public void SetCorrelationId(string correlationId) => _correlationId = correlationId;        
    }
}
