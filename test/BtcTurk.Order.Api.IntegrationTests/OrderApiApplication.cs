using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace BtcTurk.Order.Api.IntegrationTests;

internal class OrderApiApplication : WebApplicationFactory<Program>
{
    public HttpClient CreateClient(string id, bool isAdmin = false)
    {
        var client = CreateDefaultClient();
        client.DefaultRequestHeaders.Add("userid","123");
        return client;
    }
    
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            
          
        });
        return base.CreateHost(builder);
    }
}