using AutoMapper;
using POS.Application.DTOs;
using POS.Application.Interfaces;
using POS.Application.ViewModels;
using POS.Domain.Entities;
using POS.Domain.Enums;
using POS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace POS.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _uow;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public OrderService(
            IUnitOfWork uow,
            IHttpContextAccessor httpContext,
            IMapper mapper)
        {
            _uow = uow;
            _httpContext = httpContext;
            _mapper = mapper;
        }

        public async Task<Order> CreateOrderAsync(CreateOrderDto dto, int branchId)
        {
            var cashierId = _httpContext.HttpContext!
                .User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var invoiceNumber = await _uow.Orders.GenerateInvoiceNumberAsync(branchId);

            var order = new Order
            {
                InvoiceNumber = invoiceNumber,
                OrderType = dto.OrderType,
                BranchId = branchId,
                TableId = dto.TableId,
                CustomerName = dto.CustomerName,
                CustomerPhone = dto.CustomerPhone,
                CashierId = cashierId,
                Notes = dto.Notes,
                DiscountType = dto.DiscountType,
                DiscountValue = dto.DiscountValue,
            };

            decimal subTotal = 0;
            decimal taxTotal = 0;

            foreach (var item in dto.Items)
            {
                var product = await _uow.Products.GetByIdAsync(item.ProductId)
                    ?? throw new KeyNotFoundException($"Product {item.ProductId} not found");

                if (product.StockQuantity < item.Quantity)
                    throw new InvalidOperationException(
                        $"Insufficient stock for '{product.Name}'. " +
                        $"Available: {product.StockQuantity}, Requested: {item.Quantity}");

                var lineSubTotal = product.SalePrice * item.Quantity;
                var lineTax = lineSubTotal * product.TaxRate;

                order.Items.Add(new OrderItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    UnitPrice = product.SalePrice,
                    Quantity = item.Quantity,
                    TaxAmount = lineTax,
                    DiscountAmount = 0,
                    LineTotal = lineSubTotal + lineTax,
                    Notes = item.Notes,
                });

                subTotal += lineSubTotal;
                taxTotal += lineTax;

                // Deduct stock
                product.StockQuantity -= item.Quantity;
                await _uow.Products.UpdateAsync(product);
                await _uow.StockMovements.RecordMovementAsync(
                    product.Id, -item.Quantity,
                    StockMovementType.Sale, invoiceNumber);
            }

            var discount = CalculateDiscount(subTotal, dto.DiscountType, dto.DiscountValue);

            order.SubTotal = subTotal;
            order.TaxAmount = taxTotal;
            order.DiscountAmount = discount;
            order.TotalAmount = subTotal + taxTotal - discount;
            order.Status = dto.OrderType == OrderType.Retail
                ? OrderStatus.Completed
                : OrderStatus.Pending;

            foreach (var p in dto.Payments)
                order.Payments.Add(new Payment
                {
                    Method = p.Method,
                    Amount = p.Amount,
                    ReferenceNumber = p.Reference,
                });

            if (dto.TableId.HasValue && order.Status == OrderStatus.Completed)
            {
                var table = await _uow.Tables.GetByIdAsync(dto.TableId.Value);
                if (table != null)
                {
                    table.Status = TableStatus.Cleaning;
                    await _uow.Tables.UpdateAsync(table);
                }
            }

            await _uow.Orders.AddAsync(order);
            await _uow.CommitAsync();
            return order;
        }

        public async Task<OrderDetailViewModel?> GetOrderWithDetailsAsync(int orderId)
        {
            var order = await _uow.Orders.GetWithItemsAsync(orderId);
            return order == null ? null : _mapper.Map<OrderDetailViewModel>(order);
        }

        public async Task<IEnumerable<OrderDetailViewModel>> GetOrdersByDateAsync(
            DateTime from, DateTime to, int branchId)
        {
            var orders = await _uow.Orders.GetByDateRangeAsync(from, to, branchId);
            return _mapper.Map<IEnumerable<OrderDetailViewModel>>(orders);
        }

        public async Task UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _uow.Orders.GetWithItemsAsync(orderId)
                ?? throw new KeyNotFoundException("Order not found");

            order.Status = status;

            if (status == OrderStatus.Completed || status == OrderStatus.Served)
                foreach (var item in order.Items)
                    item.Status = OrderItemStatus.Served;

            await _uow.Orders.UpdateAsync(order);
            await _uow.CommitAsync();
        }

        public async Task UpdateOrderItemStatusAsync(int orderItemId, OrderItemStatus status)
        {
            var item = await _uow.OrderItems.GetByIdAsync(orderItemId)
                ?? throw new KeyNotFoundException("Order item not found");

            item.Status = status;
            await _uow.OrderItems.UpdateAsync(item);
            await _uow.CommitAsync();
        }

        public async Task<IEnumerable<OrderDetailViewModel>> GetActiveTableOrdersAsync(int branchId)
        {
            var orders = await _uow.Orders.GetActiveTableOrdersAsync(branchId);
            return _mapper.Map<IEnumerable<OrderDetailViewModel>>(orders);
        }

        public async Task<IEnumerable<TableDto>> GetAvailableTablesAsync(int branchId)
        {
            var tables = await _uow.Tables.FindAsync(
                t => t.BranchId == branchId && t.Status == TableStatus.Available);
            return _mapper.Map<IEnumerable<TableDto>>(tables);
        }

        private static decimal CalculateDiscount(
            decimal subTotal,
            DiscountType type,
            decimal value)
            => type switch
            {
                DiscountType.Fixed => Math.Min(value, subTotal),
                DiscountType.Percentage => subTotal * (value / 100),
                _ => 0
            };
    }
}
