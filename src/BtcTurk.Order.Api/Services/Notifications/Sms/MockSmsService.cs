namespace BtcTurk.Order.Api.Services.Notifications.Sms;

public class MockSmsService : ISmsService
{
    private readonly ILogger<MockSmsService> _logger;

    public MockSmsService(ILogger<MockSmsService> logger)
    {
        _logger = logger;
    }
    public Task NotifyAsync(NotificationMessage notificationMessage)
    {
        _logger.LogInformation("Sms sent : {@notificationMessage}",notificationMessage);
        return Task.CompletedTask;
    }
}