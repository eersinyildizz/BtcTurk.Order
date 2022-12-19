namespace BtcTurk.Order.Api.Middlewares;

public class ExceptionMessage
{
    public string Message { get; set; }

    public string Code { get; set; }

    public string? TraceId { get; set; }
}