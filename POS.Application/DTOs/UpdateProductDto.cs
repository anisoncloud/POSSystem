using Microsoft.AspNetCore.Http;
using POS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace POS.Application.DTOs
{
    public class UpdateProductDto
    {
        [Required(ErrorMessage = "Product name is required")]
        [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
        public string Name { get; set; } = default!;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Purchase price must be greater than 0")]
        public decimal PurchasePrice { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Sale price must be greater than 0")]
        public decimal SalePrice { get; set; }

        [Range(0, 1, ErrorMessage = "Tax rate must be between 0 and 1 (e.g. 0.15 = 15%)")]
        public decimal TaxRate { get; set; } = 0;

        [Range(1, int.MaxValue, ErrorMessage = "Low stock threshold must be at least 1")]
        public int LowStockThreshold { get; set; } = 10;

        public UnitType UnitType { get; set; } = UnitType.Piece;
        public bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "At least one category is required")]
        public List<int> CategoryIds { get; set; } = new();
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
    }
}
