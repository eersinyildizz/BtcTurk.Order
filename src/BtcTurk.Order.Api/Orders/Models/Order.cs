using System.ComponentModel.DataAnnotations;

namespace BtcTurk.Order.Api.Orders;

public class Order : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserId { get; set; }

    public decimal Amount { get; set; }

    public int DayOfMonth { get; set; }

    public int Month { get; set; }
    
    public List<OrderNotification> OrderNotifications { get; set; }
}