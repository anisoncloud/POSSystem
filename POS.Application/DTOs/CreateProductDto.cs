using Microsoft.AspNetCore.Http;
using POS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Versioning;
using System.Text;

namespace POS.Application.DTOs
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Product name is required")]
        [MaxLength(200, ErrorMessage = "Name cannot exceed 200 character")]
        public string Name { get; set; } = default!;
        [MaxLength(1000)]
        public string? Description{get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Sale price must be gratter than 0")]
        public decimal PurchasePrice {  get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage ="Sale price must be gratter than 0")]
        public decimal SalePrice {  get; set; }
        [Range(0, 1, ErrorMessage ="Tax rate must be between 0 and 1(e.g. 0.15=15%)")]
        public decimal TaxRate {  get; set; }
        [Range(0, int.MaxValue, ErrorMessage ="Initial stock can not be Negative")]
        public int InitialStock {  get; set; }
        [Range(1, int.MaxValue, ErrorMessage ="Low stock threashold must be at least 1")]
        public int LowStockThreshold { get; set; } = 10;
        public UnitType UnitType { get; set; } = UnitType.Piece;
        [Required(ErrorMessage ="At least one category is required")]
        [MinLength(1, ErrorMessage ="Select at least one category")]
        public List<int> CategoryIds { get; set; } = new();
        public IFormFile? Image {  get; set; }
        public string? ImageUrl { get; set; }

    }
}
