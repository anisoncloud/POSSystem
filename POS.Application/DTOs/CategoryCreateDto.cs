using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace POS.Application.DTOs
{
    public class CategoryCreateDto
    {
        [Required(ErrorMessage ="Must provide category name")]
        [MaxLength(200, ErrorMessage ="Category Name must be within 200 Charechter")]
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<int> CategoryIds { get; set; } = new();
    }
}
