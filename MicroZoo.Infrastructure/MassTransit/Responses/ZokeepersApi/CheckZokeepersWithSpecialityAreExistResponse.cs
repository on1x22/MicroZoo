using Microsoft.AspNetCore.Mvc;

namespace MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi
{
    public record CheckZokeepersWithSpecialityAreExistResponse
    {
        public Guid OperationId { get; set; }
        public bool IsThereZookeeperWithThisSpeciality { get; set; }

        [Obsolete("Sould be moved to ActionResult")]
        public string ErrorMessage { get; set; }
        public IActionResult ActionResult { get; set; }
    }
}
