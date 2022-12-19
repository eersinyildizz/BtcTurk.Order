using System;
using System.Threading.Tasks;
using BtcTurk.Order.Api.Services.Notifications;
using BtcTurk.Order.Api.Services.Notifications.Email;
using BtcTurk.Order.Api.Services.Notifications.Push;
using FizzWare.NBuilder;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BtcTurk.Order.Api.Tests.Services;

public class MockPushServiceTests : TestsFor<MockPushService>
{
    [Fact]
    public void NotifyAsync_IfRequestIsNull_ThrowsException()
    {
        // Arrange
        NotificationMessage nullRequest = null;
        
        // Act && Assert
        Assert.ThrowsAsync<ArgumentNullException>(() => Instance.NotifyAsync(nullRequest));
    }
    
    [Fact]
    public async Task NotifyAsync_IfRequestValid_LogsAndCompletetask()
    {
        // Arrange
        var msg = Builder<NotificationMessage>.CreateNew().Build();

        // Act 
        await Instance.NotifyAsync(msg);
        
        // Assert
        GetMockFor<ILogger<MockPushService>>()
            .VerifyLog(LogLevel.Information,Times.Once(),"Push sent*");
    }
}