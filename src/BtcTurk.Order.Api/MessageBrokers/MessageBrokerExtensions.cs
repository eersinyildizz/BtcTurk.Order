using EasyNetQ;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace BtcTurk.Order.Api.MessageBrokers;

public static class MessageBrokerExtensions
{
    public static WebApplicationBuilder AddMessageBrokers(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.Configure<EventOptions>(builder.Configuration.GetSection(EventOptions.SectionName));
        builder.Services.TryAddSingleton<IConsumer, EasyNetQQueueClient>();
        builder.Services.TryAddSingleton<IPublisher, EasyNetQQueueClient>();
        builder.Services.RegisterEasyNetQ((sp) =>
        {
            var rabbitMqOptions = sp.Resolve<IOptions<EventOptions>>().Value.RabbitMq;
            var configuration = new ConnectionConfiguration
            {
                UserName = rabbitMqOptions.Username,
                Password = rabbitMqOptions.Password,
                VirtualHost =rabbitMqOptions.VirtualHost,
                Port = rabbitMqOptions.Port
            };
            configuration.Hosts.Add(new HostConfiguration
            {
                Host = rabbitMqOptions.Host
            });
            return configuration;
        });
        
        return builder;
    }

    public static async Task<IApplicationBuilder> ConsumeAsync(this IApplicationBuilder applicationBuilder)
    {
        var consumer = applicationBuilder.ApplicationServices.GetRequiredService<IConsumer>();
        await consumer.InitializeAsync();
        return applicationBuilder;
    }
}