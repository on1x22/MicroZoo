using MicroZoo.Infrastructure.Models.Specialities;

namespace MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi
{
    public record GetSpecialityResponse
    {
        public Guid OperationId { get; set; }
        public Speciality Speciality { get; set; }
        public string ErrorMessage { get; set; }
    }
}
