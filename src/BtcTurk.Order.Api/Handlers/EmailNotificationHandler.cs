using BtcTurk.Order.Api.Data;
using BtcTurk.Order.Api.Orders;
using BtcTurk.Order.Api.Services.Notifications;
using BtcTurk.Order.Api.Services.Notifications.Email;

namespace BtcTurk.Order.Api.Handlers;

/// <summary>
/// Email Notification Handler interface
/// </summary>
public interface IEmailNotificationHandler : IEventHandler{}

/// <summary>
/// This class contains implementation of email notification handler  
/// </summary>
public class EmailNotificationHandler : AbstractNotificationHandler , IEmailNotificationHandler
{
    private readonly IEmailService _emailService;

    /// <summary>
    /// Default const.
    /// </summary>
    /// <param name="emailService"></param>
    /// <param name="orderDbContext"></param>
    public EmailNotificationHandler(IEmailService emailService, OrderDbContext orderDbContext) : base(orderDbContext)
    {
        _emailService = emailService;
    }
    
    /// <inheritdoc/>
    public override async Task InternalHandleAsync(NotificationMessage notificationMessage)
    {
        await _emailService.NotifyAsync(notificationMessage);
    }

    /// <inheritdoc/>
    public override (string queueName, string headerArgument) GetQueueOptions =>
        ("email-notification", NotificationType.Email.ToString());
}