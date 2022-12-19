using BtcTurk.Order.Api.Configurations;
using BtcTurk.Order.Api.CurrentUserProvider;
using BtcTurk.Order.Api.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BtcTurk.Order.Api.NotificationHistories;


internal static class NotificationHistoryApi
{
    public static RouteGroupBuilder MapNotificationHistories(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/notification-history");

        group.WithTags("notification-history");
        // Rate limit all of the APIs
        group.RequirePerUserRateLimit();

        group.MapGet("/{orderId}", async Task<Results<Ok<List<NotificationHistory>>, NotFound>>(OrderDbContext db, Guid orderId, ICurrentUser currentUser) =>
        {
            var result = await db.NotificationHistories.Where(p => p.OrderId == orderId && p.UserId == currentUser.Id).AsNoTracking().ToListAsync();
            if (result.Any())
            {
                return TypedResults.Ok(result);
            }
            return TypedResults.NotFound();
        });
        
        group.MapGet("/", async Task<Results<Ok<List<NotificationHistory>>, NotFound>>(OrderDbContext db, ICurrentUser currentUser) =>
        {
            var result = await db.NotificationHistories.Where(p => p.UserId == currentUser.Id).AsNoTracking()
                .ToListAsync();
            if (result.Any())
            {
                return TypedResults.Ok(result);
            }
            return TypedResults.NotFound();
        });


        return group;
    }
}