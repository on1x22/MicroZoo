namespace MicroZoo.Gateway.CorrelationIdGenerator
{
    public interface __ICorrelationIdGenerator
    {
        string GetCorrelationId();
        void SetCorrelationId(string correlationId);
    }
}
