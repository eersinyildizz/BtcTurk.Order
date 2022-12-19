using System.ComponentModel.DataAnnotations;
using BtcTurk.Order.Api.Orders;

namespace BtcTurk.Order.Api.NotificationHistories;


public class NotificationHistory : BaseEntity
{
    [Key]
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public string UserId { get; set; }
    
    public string NotificationType { get; set; }
    public string? Message { get; set; }
}
