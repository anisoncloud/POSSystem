using POS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Domain.Entities
{
    public class Table : BaseEntity
    {
        public string TableNumber { get; set; } = default!;
        public int Capacity { get; set; }
        public TableStatus Status { get; set; } = TableStatus.Available;
        public int BranchId { get; set; }
        public Branch Branch { get; set; } = default!;
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
