using Microsoft.AspNetCore.Mvc;

namespace MicroZoo.ZookeepersApi.Tests.UnitTests
{
    public static class ActionResultExtensions
    {
        public static TOut ReturnActionResultValue<TIn, TOut>(this IActionResult actionResult)
            where TIn : ObjectResult
            where TOut : class
        {
            var objectResult = actionResult as TIn ;

            var result = objectResult.Value;
            
            return (TOut)(result != null ? result : default);
        }
    }
}
