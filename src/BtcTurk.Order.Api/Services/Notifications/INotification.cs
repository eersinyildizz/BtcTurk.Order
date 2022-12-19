namespace BtcTurk.Order.Api.Services.Notifications;

public interface INotification
{
    Task NotifyAsync(NotificationMessage notificationMessage);
}