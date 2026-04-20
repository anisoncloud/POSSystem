using POS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.DTOs
{
    public record CreateOrderDto(
    OrderType OrderType,
    int? TableId,
    string? CustomerName,
    string? CustomerPhone,
    DiscountType DiscountType,
    decimal DiscountValue,
    string? Notes,
    List<CreateOrderItemDto> Items,
    List<CreatePaymentDto> Payments
);
    public record CreateOrderItemDto(int ProductId, int Quantity, string? Notes);
    public record CreatePaymentDto(PaymentMethod Method, decimal Amount, string? Reference);
}
