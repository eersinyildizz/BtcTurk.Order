using System;
using System.Threading.Tasks;
using BtcTurk.Order.Api.Data;
using BtcTurk.Order.Api.Handlers;
using BtcTurk.Order.Api.Orders;
using BtcTurk.Order.Api.Services.Notifications;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BtcTurk.Order.Api.Tests.Handlers;

public class AbstractNotificationHandlerTests
{
    [Fact]
    public void HandleAsync_IfRequestIsNull_ThrowsException()
    {
        // Arrange
        var testHandler = new TestNotificationHandler(GetDbContext());
        
        // Act && Assert
        Assert.ThrowsAsync<ArgumentNullException>(() => testHandler.HandleAsync(null));

    }

    [Fact]
    public async Task HandleAsync_WhenCalled_HistoryShouldAdd()
    {
        // Arrange
        var dbContext = GetDbContext();
        var orderId = Guid.NewGuid();
        var order = new Orders.Order
        {
            Id = orderId,
            UserId = "123"
        };
        dbContext.Orders.Add(order);
        dbContext.OrderNotifications.Add(new OrderNotification
        {
            NotificationType = NotificationType.Email,
            Order = order
        });
        await dbContext.SaveChangesAsync();
        
        var testHandler = new TestNotificationHandler(dbContext);
        var notificationMessage = new NotificationMessage
        {
            OrderId = orderId,
            NotificationType = NotificationType.Sms
        };
        
        
        // Act
        var result = testHandler.HandleAsync(notificationMessage);
        
        // Assert
        Assert.NotEqual(dbContext.NotificationHistories.Local.Count,0);

    }

    
    private OrderDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<OrderDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new OrderDbContext(options);
    }
}



internal class TestNotificationHandler : AbstractNotificationHandler
{
    public TestNotificationHandler(OrderDbContext dbContext) : base(dbContext)
    {
    }

    public override (string queueName, string headerArgument) GetQueueOptions => ("test", NotificationType.Email.ToString());
    public override Task InternalHandleAsync(NotificationMessage notificationMessage)
    {
        return Task.CompletedTask;
    }
}