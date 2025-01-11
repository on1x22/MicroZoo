using Microsoft.AspNetCore.Mvc;
using MicroZoo.Infrastructure.MassTransit.Responses;

namespace MicroZoo.Infrastructure.MassTransit
{
    public interface IRabbitMqResponseErrorsHandler
    {
        IActionResult GetActionResult(IResponseWithError response);
    }
}
