namespace PartsCom.Application.Commands.CreateOrder;

public sealed record CreateOrderCommandResponse(
    Guid OrderId,
    string OrderNumber,
    decimal TotalAmount
);
