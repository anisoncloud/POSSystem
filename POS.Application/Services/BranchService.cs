using AutoMapper;
using POS.Application.DTOs;
using POS.Application.Interfaces;
using POS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.Services
{
    public class BranchService : IBranchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public Task<BranchDto> CreateAsync(CreateBranchDto dto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BranchDto>> GetAllActiveAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BranchDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BranchDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task ToggleStatusAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BranchDto> UpdateAsync(int id, UpdateBranchDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
