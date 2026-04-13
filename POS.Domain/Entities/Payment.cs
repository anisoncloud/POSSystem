using POS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public int OrderId { get; set; }
        public Order Order { get; set; } = default!;
        public PaymentMethod Method { get; set; }
        public decimal Amount { get; set; }
        public string? ReferenceNumber {  get; set; }
        public DateTime PaidAt { get; set; } = DateTime.UtcNow;
    }
}
