using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace POS.Application.DTOs
{
    public class UpdateBranchDto
    {
        [Required(ErrorMessage = "Branch name is required")]
        [MaxLength(200)]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "Address is required")]
        [MaxLength(500)]
        public string Address { get; set; } = default!;

        [Required(ErrorMessage = "Phone is required")]
        [MaxLength(50)]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string Phone { get; set; } = default!;

        public bool IsActive { get; set; } = true;
    }
}
