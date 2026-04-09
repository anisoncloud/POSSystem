using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int? ParentCategoryId { get; set; }
        public ICollection<Category> SubCategory { get; set; } = new List<Category>();
    }
}
