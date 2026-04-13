using POS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Domain.Entities
{
    public class StockMovement : BaseEntity
    {
        public int ProductId {  get; set; }
        public Product Product { get; set; } = default!;
        public int Quantity {  get; set; }
        public StockMovementType MovementType { get; set; }
        public string Reference { get; set; }
        public string? Notes {  get; set; }
        public int BalanceAfter {  get; set; }
    }
}
