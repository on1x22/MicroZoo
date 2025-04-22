using Microsoft.AspNetCore.Mvc;
using MicroZoo.Infrastructure.MassTransit.Responses;

namespace MicroZoo.Infrastructure.MassTransit
{
    /// <summary>
    /// Provides processing <see cref="IResponseWithError"/> objects and returns 
    /// <see cref="IActionResult"/> with error
    /// </summary>
    public interface IRabbitMqResponseErrorsHandler
    {
        /// <summary>
        /// Processes <see cref="IResponseWithError"/> object and returns 
        /// <see cref="IActionResult"/> with description of error
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        IActionResult GetActionResult(IResponseWithError response);
    }
}
