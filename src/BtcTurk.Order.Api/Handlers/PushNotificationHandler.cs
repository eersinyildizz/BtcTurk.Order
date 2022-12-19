using BtcTurk.Order.Api.Data;
using BtcTurk.Order.Api.Orders;
using BtcTurk.Order.Api.Services.Notifications;
using BtcTurk.Order.Api.Services.Notifications.Push;

namespace BtcTurk.Order.Api.Handlers;

public interface IPushNotificationHandler : IEventHandler{}

public class PushNotificationHandler : AbstractNotificationHandler, IPushNotificationHandler
{
    private readonly IPushService _pushService;

    public PushNotificationHandler(IPushService pushService, OrderDbContext dbContext) : base(dbContext)
    {
        _pushService = pushService;
    }

    /// <inheritdoc/>
    protected override async Task InternalHandleAsync(NotificationMessage notificationMessage)
    {
        await _pushService.NotifyAsync(notificationMessage);
    }

    /// <inheritdoc/>
    public override (string queueName, string headerArgument) GetQueueOptions =>
        ("push-notification", NotificationType.Push.ToString());
}