using Mapster;

namespace BtcTurk.Order.Api.Orders;

public static class TodoMappingExtensions
{
    public static OrderDto AsOrderDto(this Order order)
    {
        return order.Adapt<OrderDto>();
    }
    
    public static List<OrderDto> AsOrderDtoList(this List<Order> order)
    {
        return order.Adapt<List<OrderDto>>();
    }
}