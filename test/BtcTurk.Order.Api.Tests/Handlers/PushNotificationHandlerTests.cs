using System;
using System.Threading.Tasks;
using BtcTurk.Order.Api.Data;
using BtcTurk.Order.Api.Handlers;
using BtcTurk.Order.Api.Orders;
using BtcTurk.Order.Api.Services.Notifications;
using BtcTurk.Order.Api.Services.Notifications.Push;
using FizzWare.NBuilder;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace BtcTurk.Order.Api.Tests.Handlers;

public class PushNotificationHandlerTests 
{
    [Fact]
    public async Task InternalHandleAsync_WhenCalled()
    {
        // Arrange
        var pushMock = new Mock<IPushService>();
        var pushHandler = new PushNotificationHandler(pushMock.Object, GetDbContext());
        var msg = Builder<NotificationMessage>.CreateNew().Build();
        
        // Act
        await pushHandler.InternalHandleAsync(msg);
        
        // Assert
        pushMock.Verify(p=>p.NotifyAsync(msg),Times.Once);
    }
    
    [Fact]
    public async Task GetQueueOptions_WhenCalled_ResultShouldBePush()
    {
        // Arrange
        var pushMock = new Mock<IPushService>();
        var pushHandler = new PushNotificationHandler(pushMock.Object, GetDbContext());
        
        // Act
        var res = pushHandler.GetQueueOptions;
        
        // Assert
        Assert.Equal(res.queueName, "push-notification");
        Assert.Equal(res.headerArgument, NotificationType.Push.ToString());
    }
    
    private OrderDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<OrderDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new OrderDbContext(options);
    }
}