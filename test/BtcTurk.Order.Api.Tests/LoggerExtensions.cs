using System;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Moq;

namespace BtcTurk.Order.Api.Tests;

/// <summary>
/// This class contains mock of <see cref="ILogger{TCategoryName}"/> extensions methods
/// </summary>
public static class LoggerExtensions
{
    /// <summary>
    /// Verify mock log
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="level"></param>
    /// <param name="times"></param>
    /// <param name="regex"></param>
    /// <typeparam name="T"></typeparam>
    public static void VerifyLog<T>(this Mock<ILogger<T>> logger, LogLevel level, Times times, string? regex = null) =>
        logger.Verify(m => m.Log(
                level,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((x, y) => regex == null || Regex.IsMatch(x.ToString(), regex)),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            times);
}