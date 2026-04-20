using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace POS.Application.DTOs
{
    public class CreateSupplierDto
    {
        [Required, MaxLength(200)]
        public string Name { get; set; } = default!;

        [MaxLength(200)]
        public string? ContactPerson { get; set; }

        [EmailAddress, MaxLength(200)]
        public string? Email { get; set; }

        [MaxLength(50)]
        public string? Phone { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }
    }
}
