using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.DTOs
{
    public class CategoryUpdateDto
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int? ParentCategoryId {  get; set; }
    }
}
