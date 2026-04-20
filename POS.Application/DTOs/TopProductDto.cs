using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.DTOs
{
    public class TopProductDto
    {
        public string ProductName { get; set; } = default!;
        public int QuantitySold { get; set; }
        public decimal Revenue { get; set; }
    }
}
