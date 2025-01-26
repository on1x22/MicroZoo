using Microsoft.AspNetCore.Mvc;
using MicroZoo.Infrastructure.MassTransit.Responses;

namespace MicroZoo.Infrastructure.MassTransit
{
    public class RabbitMqResponseErrorsHandler : IRabbitMqResponseErrorsHandler
    {
        public IActionResult GetActionResult(IResponseWithError response)
        {
            IActionResult result = new UnauthorizedResult();
            
            try
            {
                /*if (response.ResponseError != null)
                {
                    switch (response.ResponseError.ErrorCode)
                    {
                        case ErrorCodes.BadRequest400:
                            return new BadRequestObjectResult(response.ResponseError.ErrorMessage);
                        case ErrorCodes.Unauthorized401:
                            return new UnauthorizedResult();
                        case ErrorCodes.Forbiden403:
                            return new ForbidResult(response.ResponseError.ErrorMessage);
                    }
                    return new BadRequestResult();
                }*/

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
