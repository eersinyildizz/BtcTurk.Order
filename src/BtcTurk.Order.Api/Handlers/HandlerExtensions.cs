namespace BtcTurk.Order.Api.Handlers;

public static class HandlerExtensions
{
    /// <summary>
    /// Register event handler services
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <returns></returns>
    public static IServiceCollection AddHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IEmailNotificationHandler, EmailNotificationHandler>();
        serviceCollection.AddTransient<ISmsNotificationHandler, SmsNotificationHandler>();
        serviceCollection.AddTransient<IPushNotificationHandler, PushNotificationHandler>();
        
        var eventHandlers = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
            .Where(p => p.IsAssignableTo(typeof(IEventQueueOptions)) && p.IsClass && !p.IsAbstract);

        foreach (var handler in eventHandlers)
        {
            serviceCollection.AddTransient(typeof(IEventHandler), handler);
            serviceCollection.AddTransient(handler);

        }
        return serviceCollection;
    }
}