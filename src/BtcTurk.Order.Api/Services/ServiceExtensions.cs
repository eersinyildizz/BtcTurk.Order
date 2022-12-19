using BtcTurk.Order.Api.MessageBrokers;
using BtcTurk.Order.Api.Services.Notifications;
using BtcTurk.Order.Api.Services.Notifications.Email;
using BtcTurk.Order.Api.Services.Notifications.Push;
using BtcTurk.Order.Api.Services.Notifications.Sms;

namespace BtcTurk.Order.Api.Services;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);
        serviceCollection.AddTransient<IEmailService, MockEmailService>();
        serviceCollection.AddTransient<IPushService, MockPushService>();
        serviceCollection.AddTransient<ISmsService, MockSmsService>();
        return serviceCollection;
    }
}