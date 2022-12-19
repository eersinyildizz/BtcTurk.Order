namespace BtcTurk.Order.Api.MessageBrokers;

/// <summary>
/// Event options
/// </summary>
public class EventOptions
{
    /// <summary>
    /// name of the section
    /// </summary>
    public const string SectionName = "Event";

    /// <summary>
    /// Options of rabbitmq
    /// </summary>
    public RabbitMqOptions RabbitMq { get; set; } = new RabbitMqOptions();
    
    /// <summary>
    /// Number of retry 
    /// </summary>
    public int RetryCount { get; set; } = 5;
    
    
}

/// <summary>
/// Rabbitmq options
/// </summary>
public class RabbitMqOptions
{
    /// <summary>
    /// Username
    /// </summary>
    public string Username { get; set; } = "guest";

    /// <summary>
    /// Password
    /// </summary>
    public string Password { get; set; }= "guest";

    /// <summary>
    /// Virtual Host
    /// </summary>
    public string VirtualHost { get; set; } = "/";

    /// <summary>
    /// Host
    /// </summary>
    public string Host { get; set; } = "localhost";

    /// <summary>
    /// Port
    /// </summary>
    public ushort Port { get; set; } = 5672;
}