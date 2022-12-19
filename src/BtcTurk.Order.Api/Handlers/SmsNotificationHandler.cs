using BtcTurk.Order.Api.Data;
using BtcTurk.Order.Api.Orders;
using BtcTurk.Order.Api.Services.Notifications;
using BtcTurk.Order.Api.Services.Notifications.Sms;

namespace BtcTurk.Order.Api.Handlers;

/// <summary>
/// Sms Notification Handler interface
/// </summary>
public interface ISmsNotificationHandler : IEventHandler{}

/// <summary>
/// 
/// </summary>
public class SmsNotificationHandler :AbstractNotificationHandler,  ISmsNotificationHandler
{
    private readonly ISmsService _smsService;

    public SmsNotificationHandler(ISmsService smsService, OrderDbContext dbContext):base(dbContext)
    {
        _smsService = smsService;
    }
    
    /// <inheritdoc/>
    protected override async Task InternalHandleAsync(NotificationMessage notificationMessage)
    {
        await _smsService.NotifyAsync(notificationMessage);
    }
    
    /// <inheritdoc/>
    public override (string queueName, string headerArgument) GetQueueOptions =>
        ("sms-notification", NotificationType.Sms.ToString());
}