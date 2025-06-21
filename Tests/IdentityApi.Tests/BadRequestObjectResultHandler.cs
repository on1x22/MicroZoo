using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MicroZoo.IdentityApi.Tests
{
    internal class BadRequestObjectResultHandler
    {
        internal static List<string> GetErrorsFromBadRequestObjectResult(
            BadRequestObjectResult badRequestResult)
        {
            var json = JsonSerializer.Serialize(badRequestResult.Value);
            var document = JsonDocument.Parse(json);
            return document.RootElement.GetProperty("Errors").EnumerateArray()
                .Select(e => e.GetString())
                .ToList()!;
        }
    }
}
