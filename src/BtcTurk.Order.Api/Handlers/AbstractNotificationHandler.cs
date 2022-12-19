using BtcTurk.Order.Api.Data;
using BtcTurk.Order.Api.NotificationHistories;
using BtcTurk.Order.Api.Services.Notifications;
using Microsoft.EntityFrameworkCore;

namespace BtcTurk.Order.Api.Handlers;

public abstract class AbstractNotificationHandler : IEventHandler
{
    private readonly OrderDbContext _dbContext;

    protected AbstractNotificationHandler(OrderDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc/>
    public abstract (string queueName, string headerArgument) GetQueueOptions { get; }

    /// <inheritdoc/>
    public async Task HandleAsync(NotificationMessage notificationMessage)
    {
        ArgumentNullException.ThrowIfNull(notificationMessage);
        var order = await _dbContext.Orders.Include(p => p.OrderNotifications).AsNoTracking()
            .SingleOrDefaultAsync(p => p.Id == notificationMessage.OrderId);
        
        if (order is null || order.OrderNotifications.All(p => p.NotificationType.ToString() != GetQueueOptions.headerArgument))
        {
            return;
        }
        
        await InternalHandleAsync(notificationMessage);
        _dbContext.NotificationHistories.Add(new NotificationHistory
        {
            OrderId = notificationMessage.OrderId,
            Message = notificationMessage.Message,
            UserId = notificationMessage.UserId,
            NotificationType = GetQueueOptions.headerArgument
        });
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="notificationMessage"></param>
    /// <returns></returns>
    public abstract Task InternalHandleAsync(NotificationMessage notificationMessage);
}