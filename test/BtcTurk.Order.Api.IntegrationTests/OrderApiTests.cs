using System.Threading.Tasks;
using BtcTurk.Order.Api.Middlewares;
using BtcTurk.Order.Api.Orders;

namespace BtcTurk.Order.Api.IntegrationTests;

public class OrderApiTests
{
    [Fact]
    public async Task GetOrders_IfNotExist_ShouldNotBeSuccessStatus()
    {
        await using var application = new OrderApiApplication();
        var client = application.CreateClient();
        var response = await client.GetAsync("/orders");
        Assert.False(response.IsSuccessStatusCode);
    }
    
    [Fact]
    public async Task GetOrders_IfRequestAmountIsLessThanMinAmount_ShouldNotBeSuccessStatus()
    {
        await using var application = new OrderApiApplication();
        var client = application.CreateClient();
        var response = await client.PostAsJsonAsync("/orders", new OrderDto
        {
            Amount = -1
        });
        
        var exceptionDetails = await response.Content.ReadFromJsonAsync<ExceptionMessage>();
        Assert.NotNull(exceptionDetails);
        Assert.Equal("InvalidAmount!",exceptionDetails.Message);
        Assert.False(response.IsSuccessStatusCode);
        Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task GetOrders_IfRequestAmountIsGreaterThanMinAmount_ShouldNotBeSuccessStatus()
    {
        await using var application = new OrderApiApplication();
        var client = application.CreateClient();
        var response = await client.PostAsJsonAsync("/orders", new OrderDto
        {
            Amount = 10000000
        });
        
        var exceptionDetails = await response.Content.ReadFromJsonAsync<ExceptionMessage>();
        Assert.NotNull(exceptionDetails);
        Assert.Equal("InvalidAmount!",exceptionDetails.Message);
        Assert.False(response.IsSuccessStatusCode);
        Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task GetOrders_IfRequestDayOfMonthIsGreaterThanMaxDay_ShouldNotBeSuccessStatus()
    {
        await using var application = new OrderApiApplication();
        var client = application.CreateClient();
        var response = await client.PostAsJsonAsync("/orders", new OrderDto
        {
            Amount = 10000,
            DayOfMonth = 29
        });
        
        var exceptionDetails = await response.Content.ReadFromJsonAsync<ExceptionMessage>();
        Assert.NotNull(exceptionDetails);
        Assert.Equal("InvalidDay!",exceptionDetails.Message);
        Assert.False(response.IsSuccessStatusCode);
        Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task GetOrders_IfRequestDayOfMonthIsLessThanMinDay_ShouldNotBeSuccessStatus()
    {
        await using var application = new OrderApiApplication();
        var client = application.CreateClient();
        var response = await client.PostAsJsonAsync("/orders", new OrderDto
        {
            Amount = 10000,
            DayOfMonth = -1
        });
        
        var exceptionDetails = await response.Content.ReadFromJsonAsync<ExceptionMessage>();
        Assert.NotNull(exceptionDetails);
        Assert.Equal("InvalidDay!",exceptionDetails.Message);
        Assert.False(response.IsSuccessStatusCode);
        Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task GetOrders_IfRequestMonthIsInvalid_ShouldNotBeSuccessStatus()
    {
        await using var application = new OrderApiApplication();
        var client = application.CreateClient();
        var response = await client.PostAsJsonAsync("/orders", new OrderDto
        {
            Amount = 10000,
            DayOfMonth = 15,
            Month = -1
        });
        
        var exceptionDetails = await response.Content.ReadFromJsonAsync<ExceptionMessage>();
        Assert.NotNull(exceptionDetails);
        Assert.Equal("InvalidMonth!",exceptionDetails.Message);
        Assert.False(response.IsSuccessStatusCode);
        Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
    }
}