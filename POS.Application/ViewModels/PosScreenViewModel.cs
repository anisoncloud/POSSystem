using POS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.ViewModels
{
    public class PosScreenViewModel
    {
        public IEnumerable<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
        public IEnumerable<TableDto> Tables { get; set; } = new List<TableDto>();
        public IEnumerable<ProductDto> Products { get; set; } = new List<ProductDto>();
    }
}
