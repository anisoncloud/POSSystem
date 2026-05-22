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
    public class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public SupplierService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _uow = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SupplierDto> CreateSupplierAsync(CreateSupplierDto dto)
        {
            if (dto==null)
            {
                throw new ArgumentNullException(nameof(dto)); 
            }
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new ArgumentException("Suppliers Can not be empty", nameof(dto.Name));   
            }
            var existing = _uow.Suppliers.GetByNameAsync(dto.Name);
            if (existing !=null )
            {
                throw new InvalidOperationException(
                    $"This Supplier Name {dto.Name} You provided is already exists"
                    );
            }
            var supplier = _mapper.Map<Supplier>(dto);
            await _uow.Suppliers.AddAsync(supplier);
            await _uow.CommitAsync();
            return _mapper.Map<SupplierDto>(supplier);
        }

        public Task<IEnumerable<SupplierDto>> GetAllSupplierAsync()
        {
            throw new NotImplementedException();
        }

        public Task<SupplierDto> GetSupplerAsync(int supplierId)
        {
            throw new NotImplementedException();
        }

        public Task<SupplierDto?> GetSupplerWithProductAsync(int supplierId)
        {
            throw new NotImplementedException();
        }
    }
}
