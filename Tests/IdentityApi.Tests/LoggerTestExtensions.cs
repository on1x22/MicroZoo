using Microsoft.Extensions.Logging;
using Moq;

namespace MicroZoo.IdentityApi.Tests
{
    internal static class LoggerTestExtensions
    {
        internal static void VerifyLog<T>(this Mock<ILogger<T>> loggerMock, LogLevel level,
            string message, Times times)
        {
            loggerMock.Verify(x => 
                x.Log(
                    level,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains(message)),
                    It.IsAny<Exception>(),
                    /*It.Is<Func<It.IsAnyType, Exception, string>>((_, _) => true)*/
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                times);
        }
    }
}
