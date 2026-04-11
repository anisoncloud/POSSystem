using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Domain.Entities
{
    public class ProductCategory
    {
        public int ProductId {  get; set; }
        public Product Product { get; set; } = default!;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = default!;
    }
}
