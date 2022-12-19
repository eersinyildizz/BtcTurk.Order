using System.Net.Http;
using BtcTurk.Order.Api.MessageBrokers;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Moq;

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
            var mockBus = new Mock<IBus>();
            services.AddSingleton<IBus>(mockBus.Object);
            services.AddSingleton<IConsumer>(new Mock<IConsumer>().Object);
        });
        return base.CreateHost(builder);
    }
}