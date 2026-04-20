using POS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace POS.Application.DTOs
{
    public class StockAdjustmentDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public StockMovementType MovementType { get; set; }

        public string? Reference { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }
    }
}
