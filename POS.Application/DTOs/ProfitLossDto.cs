using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.DTOs
{
    public class ProfitLossDto
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public decimal Revenue { get; set; }
        public decimal COGS { get; set; }
        public decimal GrossProfit { get; set; }
        public decimal GrossMargin { get; set; }
    }
}
