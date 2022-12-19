using BtcTurk.Order.Api.Services.Notifications;

namespace BtcTurk.Order.Api.Handlers;

/// <summary>
/// Event Handler interface
/// </summary>
public interface IEventHandler : IEventQueueOptions
{
    /// <summary>
    /// Handler message 
    /// </summary>
    /// <param name="notificationMessage"></param>
    /// <returns></returns>
    public Task HandleAsync(NotificationMessage notificationMessage);
}

public interface IEventQueueOptions
{
    public (string queueName, string headerArgument) GetQueueOptions { get; }
}