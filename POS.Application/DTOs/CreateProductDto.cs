using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace POS.Application.DTOs
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Product name is required")]
        [MaxLength(200, ErrorMessage = "Name cannot exceed 200 character")]
        public string Name { get; set; } = default!;

    }
}
