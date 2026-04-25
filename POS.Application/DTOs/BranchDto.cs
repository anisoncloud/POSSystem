using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.DTOs
{
    public class BranchDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TotalUsers {  get; set; }
        public int TotalProducts {  get; set; }
        public int TotalOrders {  get; set; }

    }
}
