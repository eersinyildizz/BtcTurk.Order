using BtcTurk.Order.Api.Orders;

namespace BtcTurk.Order.Api.Services.Notifications;

/// <summary>
/// General Notification Message
/// </summary>
public class NotificationMessage
{
    public string Message { get; set; }
    public Guid OrderId { get; set; }
    public string UserId { get; set; }

    public NotificationType NotificationType { get; set; }
}

