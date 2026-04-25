using POS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace POS.Application.Interfaces
{
    public interface IBranchService 
    {
        Task<IEnumerable<BranchDto>> GetAllAsync();
        Task<IEnumerable<BranchDto>> GetAllActiveAsync();
        Task<BranchDto> GetByIdAsync(int id);
        Task<BranchDto> CreateAsync(CreateBranchDto dto);
        Task<BranchDto> UpdateAsync(int id, UpdateBranchDto dto);
        Task DeleteAsync(int id);
        Task ToggleStatusAsync(int id);

    }
}
