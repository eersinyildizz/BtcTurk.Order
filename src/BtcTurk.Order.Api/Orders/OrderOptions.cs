namespace BtcTurk.Order.Api.Orders;

public class OrderOptions
{
    public const string SectionName = "Order";

    public Day Day { get; set; } = new Day();
    
    public Amount Amount { get; set; } = new Amount();
}

public class Amount
{
    public decimal Min { get; set; } = 100;

    public decimal Max { get; set; } = 20000;
}

public class Day
{
    public int Min { get; set; } = 1;

    public int Max { get; set; } = 28;
}