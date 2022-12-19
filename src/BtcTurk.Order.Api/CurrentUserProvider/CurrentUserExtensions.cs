namespace BtcTurk.Order.Api.CurrentUserProvider;

public static class CurrentUserExtensions
{
    public static IServiceCollection AddCurrentUser(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, HttpHeaderCurrentUserProvider>();
        return services;
    }
}