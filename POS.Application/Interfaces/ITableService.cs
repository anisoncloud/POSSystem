using POS.Application.DTOs;
using POS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.Interfaces
{
    public interface ITableService
    {
        Task<IEnumerable<TableDto>> GetAllTablesAsync(int branchId);
        Task<TableDto> GetByIdAsync(int id);
        Task<TableDto> CreateTableAsync(CreateTableDto dto, int branchId);
        Task<TableDto> UpdateTableAsync(int id, UpdateTableDto dto);
        Task UpdateTableStatusAsync(int id, TableStatus status);
        Task DeleteTableAsync(int id);
    }
}
