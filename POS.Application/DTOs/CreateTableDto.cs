using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace POS.Application.DTOs
{
    public class CreateTableDto
    {
        [Required, MaxLength(20)]
        public string TableNumber { get; set; } = default!;

        [Required, Range(1, 100)]
        public int Capacity { get; set; }
    }
}
