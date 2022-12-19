namespace BtcTurk.Order.Api.Orders;

public class OrderDto
{
    public Guid Id { get; set; }
    
    public decimal Amount { get; set; }

    public int DayOfMonth { get; set; }

    public int Month { get; set; }

    public DateTime CreatedAt { get; set; }
    public List<OrderNotificationDto> OrderNotifications { get; set; }
    
}