using POS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.Interfaces
{
    public interface ISupplierService
    {
        Task<SupplierDto?> GetByNameAsynic(string name);
        Task<IEnumerable<SupplierDto>> GetAllActiveAsync();
        Task<SupplierDto> CreateSupplierAsync(CreateSupplierDto dto);
    }
}
