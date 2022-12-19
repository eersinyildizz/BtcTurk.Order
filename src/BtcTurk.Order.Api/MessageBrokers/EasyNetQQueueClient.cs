using BtcTurk.Order.Api.Exceptions;
using BtcTurk.Order.Api.Handlers;
using BtcTurk.Order.Api.Orders;
using BtcTurk.Order.Api.Services.Notifications;
using EasyNetQ;
using EasyNetQ.Topology;
using Microsoft.Extensions.Options;
using IBus = EasyNetQ.IBus;

namespace BtcTurk.Order.Api.MessageBrokers;

public class EasyNetQQueueClient : IPublisher, IConsumer
{
    private readonly IBus _bus;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IOptions<EventOptions> _eventOptionsAccessors;
    private readonly ILogger<EasyNetQQueueClient> _logger;
    private EventOptions EventOptions => _eventOptionsAccessors.Value;

    public EasyNetQQueueClient(IBus bus, IServiceScopeFactory serviceScopeFactory,
        IOptions<EventOptions> eventOptionsAccessors, ILogger<EasyNetQQueueClient> logger)
    {
        _bus = bus;
        _serviceScopeFactory = serviceScopeFactory;
        _eventOptionsAccessors = eventOptionsAccessors;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        await _bus.Advanced.ExchangeDeclareAsync("notification", ExchangeType.Header, true);
        var eventHandlers = scope.ServiceProvider.GetRequiredService<IEnumerable<IEventHandler>>();
        foreach (var handlerEvent in eventHandlers)
        {
            var queue = await _bus.Advanced.QueueDeclareAsync(handlerEvent.GetQueueOptions.queueName, opt =>
            {
                opt.AsDurable(true);
                opt.AsAutoDelete(false);
            });

            await _bus.Advanced.BindAsync(new Exchange("notification", ExchangeType.Header),
                new EasyNetQ.Topology.Queue(handlerEvent.GetQueueOptions.queueName), string.Empty,
                new Dictionary<string, object>
                {
                    [$"notification-type-{handlerEvent.GetQueueOptions.headerArgument}"] =
                        handlerEvent.GetQueueOptions.headerArgument
                });

            _bus.Advanced.Consume<NotificationMessage>(queue, async (msg, info) =>
            {
                try
                {
                    using var messageScope = _serviceScopeFactory.CreateScope();
                    var eventHandler =
                        (IEventHandler)messageScope.ServiceProvider.GetRequiredService(handlerEvent.GetType());
                    await eventHandler.HandleAsync(msg.Body);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error on consume message with message : {@msg}", msg);
                    if (e is not BusinessException)
                    {
                        var tryCount = msg.Properties.Headers.TryGetValue("TryCount", out var tryCountObject)
                            ? (int)tryCountObject
                            : 1;
                        ++tryCount;
                        if (tryCount < EventOptions.RetryCount)
                        {
                            msg.WithDelay(TimeSpan.FromMicroseconds(Math.Pow(2, tryCount)));
                            msg.Properties.Headers.Remove("TryCount");
                            msg.Properties.Headers.TryAdd("TryCount", tryCount);
                            await _bus.Advanced.PublishAsync(new Exchange("notification", ExchangeType.Header),
                                string.Empty,
                                false, msg);
                        }
                    }
                }
            });
        }
    }

    public async Task PublishAsync<TMessage>(TMessage message, List<NotificationType> notificationTypes)
    {
        var msg = new Message<TMessage>(message);
        foreach (var notificationType in notificationTypes)
        {
            msg.Properties.Headers.Add($"notification-type-{notificationType.ToString()}", notificationType.ToString());
        }

        await _bus.Advanced.PublishAsync(new Exchange("notification", ExchangeType.Header), string.Empty, false, msg);
    }
}