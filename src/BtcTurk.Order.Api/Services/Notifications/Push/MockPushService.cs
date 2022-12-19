namespace BtcTurk.Order.Api.Services.Notifications.Push;

public class MockPushService : IPushService
{
    private readonly ILogger<MockPushService> _logger;

    public MockPushService(ILogger<MockPushService> logger)
    {
        _logger = logger;
    }
    public Task NotifyAsync(NotificationMessage notificationMessage)
    {
        _logger.LogInformation("Push sent : {@notificationMessage}",notificationMessage);
        return Task.CompletedTask;
    }
}