namespace MicroZoo.Infrastructure.CorrelationIdGenerator
{
    public interface ICorrelationIdGenerator
    {
        string GetCorrelationId();
        void SetCorrelationId(string correlationId);
    }
}
