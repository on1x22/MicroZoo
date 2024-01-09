using MicroZoo.Infrastructure.Models.Animals;

namespace MicroZoo.Infrastructure.MassTransit.Responses
{
    public record GetAnimalsResponse
    {
        public Guid OperationId { get; set; }
        public List<Animal> Animals { get; set; }
        public string ErrorMessage { get; set; }
    }
}
