using Microsoft.AspNetCore.Mvc;

namespace MicroZoo.AuthService.Models
{
    /// <summary>
    /// Consist data about access and returned result
    /// </summary>
    /// <param name="IsAccessAllowed"></param>
    /// <param name="Result"></param>
    public record AccessResult(bool IsAccessAllowed, IActionResult Result);
}
