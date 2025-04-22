using Microsoft.AspNetCore.Mvc;
using MicroZoo.Infrastructure.MassTransit.Responses;

namespace MicroZoo.Infrastructure.MassTransit
{
    /// <summary>
    /// Provides processing <see cref="IResponseWithError"/> objects and returns 
    /// <see cref="IActionResult"/> with error
    /// </summary>
    public class RabbitMqResponseErrorsHandler : IRabbitMqResponseErrorsHandler
    {
        /// <summary>
        /// Processes <see cref="IResponseWithError"/> object and returns 
        /// <see cref="IActionResult"/> with description of error
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public IActionResult GetActionResult(IResponseWithError response)
        {
            IActionResult result = new UnauthorizedResult();
            
            try
            {
                if (response.ErrorCode != null)
                {
                    switch (response.ErrorCode)
                    {
                        case ErrorCodes.BadRequest400:
                            return new BadRequestObjectResult(response.ErrorMessage!);
                        case ErrorCodes.Unauthorized401:
                            return new UnauthorizedResult();
                        case ErrorCodes.Forbiden403:
                            return new ForbidResult(response.ErrorMessage!);
                        case ErrorCodes.NotFound404:
                            return new NotFoundObjectResult(response.ErrorMessage);
                        case ErrorCodes.InternalServerError500:
                            return new BadRequestObjectResult(response.ErrorMessage!);
                    }
                    return new BadRequestResult();
                }
            }
            catch (Exception)
            {
                result = new BadRequestResult();
            }
            return result;
        }
    }
}
