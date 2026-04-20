using POS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.DTOs
{
    public class StockMovementDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        public int Quantity { get; set; }
        public StockMovementType MovementType { get; set; }
        public string? Reference { get; set; }
        public string? Notes { get; set; }
        public int BalanceAfter { get; set; }
        public DateTime CreatedAt { get; set; }

        public string MovementTypeDisplay => MovementType switch
        {
            StockMovementType.Purchase => "Purchase",
            StockMovementType.Sale => "Sale",
            StockMovementType.Adjustment => "Adjustment",
            StockMovementType.Return => "Return",
            StockMovementType.Waste => "Waste",
            _ => MovementType.ToString()
        };

        public string QuantityDisplay => Quantity > 0
            ? $"+{Quantity}"
            : Quantity.ToString();
    }
}
