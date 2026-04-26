using AutoMapper;
using POS.Application.DTOs;
using POS.Application.Interfaces;
using POS.Domain.Entities;
using POS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.Services
{
    public class BranchService : IBranchService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public async Task<BranchDto> CreateAsync(CreateBranchDto dto)
        {
            var existing = await _uow.Branches.GetByNameAsync(dto.Name);
            if (existing !=null)
            {
                throw new InvalidOperationException(
                    $"A branch with the name'{dto.Name}' is already exists");
            }
            var branch = _mapper.Map<Branch>(dto);
            await _uow.Branches.AddAsync(branch);
            await _uow.CommitAsync();
            return _mapper.Map<BranchDto>(branch);
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BranchDto>> GetAllActiveAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BranchDto>> GetAllAsync()
        {
            var branches = await _uow.Branches.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<BranchDto>>(branches).ToList();
            foreach (var dto in dtos) 
            {
                dto.TotalUsers = (await _uow.Branches.HasUserAsync(dto.Id)) ? 1 : 0;
                dto.TotalProducts = (await _uow.Branches.HasProductsAsync(dto.Id)) ? 1 : 0;
            }
            return dtos;
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
