namespace BtcTurk.Order.Api.Orders;

public class OrderNotification : BaseEntity
{
    public int Id { get; set; }
    public NotificationType NotificationType { get; set; }

    public Guid OrderId { get; set; }
    public Order Order { get; set; }
}

public class OrderNotificationDto
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }
    public NotificationType NotificationType { get; set; }
}