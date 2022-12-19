using System.Threading.Tasks;

namespace BtcTurk.Order.Api.IntegrationTests;

public class OrderApiTests
{
    [Fact]
    public async Task CanCreateAUser()
    {
        await using var application = new OrderApiApplication();

        var client = application.CreateClient();
        var response = await client.GetAsync("/orders");
        Assert.False(response.IsSuccessStatusCode);
    }
}