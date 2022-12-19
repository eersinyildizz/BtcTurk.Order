namespace BtcTurk.Order.Api.Orders;

/// <summary>
/// Notification Type
/// </summary>
public enum NotificationType
{
    /// <summary>
    /// Email
    /// </summary>
    Email = 0,
    
    /// <summary>
    /// Sms
    /// </summary>
    Sms = 1,
    
    /// <summary>
    /// Push
    /// </summary>
    Push = 2
}