namespace BtcTurk.Order.Api.Services.Notifications.Email;

public class MockEmailService : IEmailService
{
    private readonly ILogger<MockEmailService> _logger;

    public MockEmailService(ILogger<MockEmailService> logger)
    {
        _logger = logger;
    }
    
    public Task NotifyAsync(NotificationMessage notificationMessage)
    {
        _logger.LogInformation("Email sent : {@notificationMessage}",notificationMessage);
        return Task.CompletedTask;
    }
}