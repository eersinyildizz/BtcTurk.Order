using System.Threading.Tasks;

namespace BtcTurk.Order.Api.IntegrationTests;

public class NotificationHistoryApiTests
{
    [Fact]
    public async Task GetNotificationHistory_IfNotExist_ShouldNotBeSuccessStatus()
    {
        await using var application = new OrderApiApplication();
        var client = application.CreateClient();
        var response = await client.GetAsync("/notification-history");
        Assert.False(response.IsSuccessStatusCode);
    }
    
    [Fact]
    public async Task GetNotificationHistoryByOrderId_IfNotExist_ShouldNotBeSuccessStatus()
    {
        await using var application = new OrderApiApplication();
        var client = application.CreateClient();
        var response = await client.GetAsync("/notification-history/123");
        Assert.False(response.IsSuccessStatusCode);
    }
}