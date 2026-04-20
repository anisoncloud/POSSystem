using POS.Application.DTOs;
using POS.Application.ViewModels;
using POS.Domain.Entities;
using POS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(CreateOrderDto dto, int branchId);
        Task<OrderDetailViewModel?> GetOrderWithDetailsAsync(int orderId);
        Task<IEnumerable<OrderDetailViewModel>> GetOrdersByDateAsync(DateTime from, DateTime to, int branchId);
        Task UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task UpdateOrderItemStatusAsync(int orderItemId, OrderItemStatus status);
        Task<IEnumerable<OrderDetailViewModel>> GetActiveTableOrdersAsync(int branchId);
        Task<IEnumerable<TableDto>> GetAvailableTablesAsync(int branchId);
    }
}
