using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Domain.Entities
{
    public class Supplier : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string? ContactPerson { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
    }
}
