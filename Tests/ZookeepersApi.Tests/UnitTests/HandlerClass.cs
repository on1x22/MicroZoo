using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroZoo.ZookeepersApi.Tests.UnitTests
{
    public static class HandlerClass
    {
        public static T GetObjectResult<T>(this ActionResult<T> result)
        {
            if (result.Result != null)
                return (T)((ObjectResult)result.Result).Value;
            return result.Value;
        }
    }

    public static class ActionResultExtensions
    {
        public static T ReturnValue<T>(this ActionResult<T> result)
        {
            return result.Result == null ? result.Value : (T)((ObjectResult)result.Result).Value;
        }
    }
}
