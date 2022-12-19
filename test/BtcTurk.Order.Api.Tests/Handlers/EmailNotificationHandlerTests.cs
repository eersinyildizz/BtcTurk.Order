using System;
using System.Threading.Tasks;
using BtcTurk.Order.Api.Data;
using BtcTurk.Order.Api.Handlers;
using BtcTurk.Order.Api.Orders;
using BtcTurk.Order.Api.Services.Notifications;
using BtcTurk.Order.Api.Services.Notifications.Email;
using FizzWare.NBuilder;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace BtcTurk.Order.Api.Tests.Handlers;

public class EmailNotificationHandlerTests
{
    [Fact]
    public async Task InternalHandleAsync_WhenCalled()
    {
        // Arrange
        var emailMock = new Mock<IEmailService>();
        var emailHandler = new EmailNotificationHandler(emailMock.Object, GetDbContext());
        var msg = Builder<NotificationMessage>.CreateNew().Build();
        
        // Act
        await emailHandler.InternalHandleAsync(msg);
        
        // Assert
        emailMock.Verify(p=>p.NotifyAsync(msg),Times.Once);
    }
    
    [Fact]
    public async Task GetQueueOptions_WhenCalled_ResultShouldBeEmail()
    {
        // Arrange
        var emailMock = new Mock<IEmailService>();
        var emailHandler = new EmailNotificationHandler(emailMock.Object, GetDbContext());
        
        // Act
        var res = emailHandler.GetQueueOptions;
        
        // Assert
        Assert.Equal(res.queueName, "email-notification");
        Assert.Equal(res.headerArgument, NotificationType.Email.ToString());
    }
    
    private OrderDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<OrderDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new OrderDbContext(options);
    }
}