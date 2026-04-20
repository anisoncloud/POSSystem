using POS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.DTOs
{
    public class TableDto
    {
        public int Id { get; set; }
        public string TableNumber { get; set; } = default!;
        public int Capacity { get; set; }
        public TableStatus Status { get; set; }
        public string StatusLabel => Status.ToString();
        public bool IsAvailable => Status == TableStatus.Available;
    }
}
