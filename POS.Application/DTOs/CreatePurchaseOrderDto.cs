using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace POS.Application.DTOs
{
    public class CreatePurchaseOrderDto
    {
        [Required]
        public int SupplierId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Today;

        public string? Notes { get; set; }

        [Required, MinLength(1)]
        public List<CreatePurchaseOrderItemDto> Items { get; set; } = new();
    }
}
