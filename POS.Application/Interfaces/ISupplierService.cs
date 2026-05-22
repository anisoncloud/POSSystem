using POS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.Interfaces
{
    public interface ISupplierService
    {
        Task<SupplierDto> CreateSupplierAsync(CreateSupplierDto dto);
        Task<SupplierDto> GetSupplerAsync(int supplierId);
        Task<IEnumerable<SupplierDto>> GetAllSupplierAsync();
        Task<SupplierDto?> GetSupplerWithProductAsync(int supplierId);
    }
}
