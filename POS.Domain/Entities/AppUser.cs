using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; } = default!;
        public int BranchId {  get; set; }
        public Branch Branch { get; set; } = default!;
        public bool IsActive { get; set; } = true;

    }
}
