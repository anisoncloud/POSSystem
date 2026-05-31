using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata;
using POS.Application.DTOs;
using POS.Application.Interfaces;
using POS.Domain.Entities;
using POS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public SupplierService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<SupplierDto> CreateSupplierAsync(CreateSupplierDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }
            if (string.IsNullOrWhiteSpace(nameof(dto.Name)))
            {
                throw new ArgumentException($"Supplier name can not be empty", nameof(dto.Name));
            }
            var exists = await _uow.Suppliers.GetByNameAsync(dto.Name);
            if (exists !=null)
            {
                throw new InvalidOperationException(
                    $"A Supplier with the name'{dto.Name}' is already exists");
            }
            var supplier = _mapper.Map<Supplier>(dto);
            await _uow.Suppliers.AddAsync(supplier);
            await _uow.CommitAsync();
            return _mapper.Map<SupplierDto>(supplier);   
        }

        public async Task<IEnumerable<SupplierDto>> GetAllActiveAsync()
        {
            var supplier = await _uow.Suppliers.GetAllActiveAsync();
            return _mapper.Map<IEnumerable<SupplierDto>>(supplier);
        }

        public async Task<SupplierDto?> GetByNameAsynic(string name)
        {
            if (string.IsNullOrWhiteSpace(nameof(name)))
            {
                return null;
            }
            var suppler = await _uow.Suppliers.GetByNameAsync(name);
            return _mapper.Map<SupplierDto>(suppler);
        }
    }
}
