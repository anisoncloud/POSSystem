using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Domain.Entities
{
    public class Branch : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public bool IsActive { get; set; } = true;
        public ICollection<AppUser> Users { get; set; } = new List<AppUser>();
        //public ICollection<Product> Products { get; set; } = new List<Product>();
        //public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
