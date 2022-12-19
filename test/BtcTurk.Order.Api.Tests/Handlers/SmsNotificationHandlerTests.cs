using System;
using System.Threading.Tasks;
using BtcTurk.Order.Api.Data;
using BtcTurk.Order.Api.Handlers;
using BtcTurk.Order.Api.Orders;
using BtcTurk.Order.Api.Services.Notifications;
using BtcTurk.Order.Api.Services.Notifications.Sms;
using FizzWare.NBuilder;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace BtcTurk.Order.Api.Tests.Handlers;

public class SmsNotificationHandlerTests
{
    [Fact]
    public async Task InternalHandleAsync_WhenCalled()
    {
        // Arrange
        var smsMock = new Mock<ISmsService>();
        var smsHandler = new SmsNotificationHandler(smsMock.Object, GetDbContext());
        var msg = Builder<NotificationMessage>.CreateNew().Build();
        
        // Act
        await smsHandler.InternalHandleAsync(msg);
        
        // Assert
        smsMock.Verify(p=>p.NotifyAsync(msg),Times.Once);
    }
    
    [Fact]
    public async Task GetQueueOptions_WhenCalled_ResultShouldBeEmail()
    {
        // Arrange
        var smsMock = new Mock<ISmsService>();
        var smsHandler = new SmsNotificationHandler(smsMock.Object, GetDbContext());
        
        // Act
        var res = smsHandler.GetQueueOptions;
        
        // Assert
        Assert.Equal(res.queueName, "sms-notification");
        Assert.Equal(res.headerArgument, NotificationType.Sms.ToString());
    }
    
    private OrderDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<OrderDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new OrderDbContext(options);
    }
}