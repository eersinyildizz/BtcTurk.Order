using BtcTurk.Order.Api.Orders;

namespace BtcTurk.Order.Api.MessageBrokers;
/// <summary>
/// Publisher interface
/// </summary>
public interface IPublisher
{
    Task PublishAsync<TMessage>(TMessage message, List<NotificationType> notificationTypes);
}