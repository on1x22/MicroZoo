using MicroZoo.Infrastructure.MassTransit;
using MicroZoo.Infrastructure.MassTransit.Responses;

namespace MicroZoo.AuthService.Models
{
    /// <summary>
    /// Consist data about access and returned result
    /// </summary>
    /// <param name="IsAccessAllowed"></param>
    /// <param name="ErrorCode"></param>
    /// <param name="ErrorMessage"></param>
    public record AccessResult(bool IsAccessAllowed,
        //IActionResult Result,
        ErrorCodes? ErrorCode,
        string? ErrorMessage = null/*,
        ResponseError ResponseError*/) : IResponseWithError;   
}
