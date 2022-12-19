using BtcTurk.Order.Api.Configurations;
using BtcTurk.Order.Api.CurrentUserProvider;
using BtcTurk.Order.Api.Data;
using BtcTurk.Order.Api.Exceptions;
using BtcTurk.Order.Api.MessageBrokers;
using BtcTurk.Order.Api.Services.Notifications;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BtcTurk.Order.Api.Orders;

internal static class OrderApi
{
    public static RouteGroupBuilder MapOrders(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/orders");

        group.WithTags("Orders");
        group.RequirePerUserRateLimit();

        group.MapPost("/",
            async ValueTask<Results<Created<OrderDto>, BadRequest<string>>>(OrderDbContext db, ICurrentUser currentUser,
                IPublisher publisher, IOptions<OrderOptions> orderOptionsAccessor, [FromBody] OrderDto newOrder
            ) =>
            {
                Validate(orderOptionsAccessor.Value,newOrder);
                var order = await db.Orders.AsNoTracking().SingleOrDefaultAsync(p => p.UserId == currentUser.Id);
                if (order != null)
                {
                    BusinessException.Throw($"OrderExists");
                }

                order = new Order
                {
                    UserId = currentUser.Id,
                    DayOfMonth = newOrder.DayOfMonth,
                    Amount = newOrder.Amount,
                    Month = newOrder.Month,
                    OrderNotifications = new List<OrderNotification>()
                };
                foreach (var newOrderNotification in newOrder.OrderNotifications)
                {
                    order.OrderNotifications.Add(new OrderNotification
                    {
                        NotificationType = newOrderNotification.NotificationType
                    });
                }
                
                db.Orders.Add(order);
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (ArgumentException e) when (e.Message.StartsWith("An item with the same key has already been added")) // Hack for InMemory Db for duplication check :( 
                {
                    return TypedResults.Created($"/orders/{order.Id}", order.AsOrderDto());
                }

                await publisher.PublishAsync(new NotificationMessage
                {
                    OrderId = order.Id,
                    Message = "Order notification saved !",
                    UserId = currentUser.Id
                }, newOrder.OrderNotifications.Select(p => p.NotificationType).ToList());
                
                return TypedResults.Created($"/orders/{order.UserId}", order.AsOrderDto());
            });

        group.MapGet("/{orderId}", async Task<Results<Ok<OrderDto>, NotFound>>(OrderDbContext db, Guid orderId, ICurrentUser currentUser) =>
            {
                var result = await db.Orders.Include(p => p.OrderNotifications).AsNoTracking()
                    .SingleOrDefaultAsync(p => p.UserId == currentUser.Id && p.Id == orderId);
                if (result != null)
                {
                    return TypedResults.Ok(result.AsOrderDto());
                }

                return TypedResults.NotFound();
            });

        group.MapGet("/", async Task<Results<Ok<OrderDto>, NotFound>>(OrderDbContext db, ICurrentUser currentUser) =>
        {
            var result = await db.Orders.Include(p => p.OrderNotifications).AsNoTracking()
                .SingleOrDefaultAsync(p => p.UserId == currentUser.Id);
            if (result != null)
            {
                return TypedResults.Ok(result.AsOrderDto());
            }

            return TypedResults.NotFound();
        });

        group.MapDelete("/{orderId}",
            async Task<Results<Ok, NotFound, BadRequest>>(Guid orderId, OrderDbContext db, ICurrentUser currentUser) =>
            {
                var order =
                    await db.Orders.Include(p=>p.OrderNotifications).SingleOrDefaultAsync(p => p.UserId == currentUser.Id && p.Id == orderId);
                if (order is null)
                {
                    return TypedResults.NotFound();
                }
                db.Orders.Remove(order);
                db.OrderNotifications.RemoveRange(order.OrderNotifications);
                await db.SaveChangesAsync();
                return TypedResults.Ok();
            });

        return group;
    }

    private static void Validate(OrderOptions orderOptions, OrderDto newOrder)
    {
        if (newOrder.Amount< orderOptions.Amount.Min || newOrder.Amount > orderOptions.Amount.Max)
        {
            BusinessException.Throw($"InvalidAmount!");
        }
                
        if (newOrder.DayOfMonth < orderOptions.Day.Min || newOrder.DayOfMonth > orderOptions.Day.Max)
        {
            BusinessException.Throw($"InvalidDay!");
        }
                
        if (newOrder.Month is < 1 or > 12)
        {
            BusinessException.Throw($"InvalidMonth!");
        }

        if (newOrder.OrderNotifications.Select(p=>p.NotificationType).Distinct().Count() != newOrder.OrderNotifications.Count)
        {
            BusinessException.Throw($"InvalidNotificationType");
        }
    }
}